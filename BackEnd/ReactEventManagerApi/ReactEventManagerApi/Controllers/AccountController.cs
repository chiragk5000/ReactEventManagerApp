using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReactEventManagerApi.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ReactEventManagerApi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AccountController(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid username or password.");

            // ✅ Generate JWT
            var token = GenerateJwtToken(user);

            // ✅ Sign-in with cookie
           // await _signInManager.SignInAsync(user, isPersistent: false);

            // ✅ Return both
            return Ok(new
            {
                token,
                expiration = DateTime.UtcNow.AddHours(1),
                message = "Login successful, JWT issued and cookie set."
            });
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public async Task <ActionResult> RegisterUser(RegisterDto registerdto)
        {
            var user = new User { UserName = registerdto.Email, Email = registerdto.Email, DisplayName = registerdto.DisplayName };
            var result = await _signInManager.UserManager.CreateAsync(user, registerdto.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem();
        }

        [AllowAnonymous]
        [HttpGet("user-info")]

        public async Task<IActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();
            var user = await _signInManager.UserManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            return Ok(new
            {
                user.DisplayName,
                user.Email,
                user.Id,
                user.ImageUrl
            });


        }

        [Authorize]
        [HttpGet("user-info-token")]
        public async Task<IActionResult> GetUserInfoToken()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
                return Unauthorized();

            
            var email = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            //var username = User.Identity?.Name ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return Ok(new
            {
                user.DisplayName,
                user.Email,
                user.Id,
                user.ImageUrl
            });
            

           
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            // Find user’s refresh tokens and revoke them
            return Ok(new { message = "Logged out successfully" });
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
