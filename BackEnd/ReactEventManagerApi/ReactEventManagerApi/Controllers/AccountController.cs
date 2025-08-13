using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReactEventManagerApi.DTOs;
using System.Net;

namespace ReactEventManagerApi.Controllers
{
    public class AccountController(SignInManager<User> _signInManager) : BaseApiController
    {
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
