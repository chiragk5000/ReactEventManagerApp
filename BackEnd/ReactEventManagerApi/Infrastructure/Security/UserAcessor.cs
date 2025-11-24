using Infrastructure.DbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Security
{
    public class UserAcessor(IHttpContextAccessor httpContextAcessor, AppDbContext dbContext) : IUserAcessor
    {
        public async Task<User> GetUserAsync()
        {
            var email = GetUserNameClaim();

            if (string.IsNullOrWhiteSpace(email))
                throw new UnauthorizedAccessException("No user is logged in");

            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.Email.Trim().ToLower() == email.Trim().ToLower());

            if (user == null)
                throw new UnauthorizedAccessException("User not found in database");

            return user;
        }

        public string GetUserNameClaim()
        {

            var claim = httpContextAcessor.HttpContext?
        .User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(claim))
                throw new UnauthorizedAccessException("No user claim found");

            return claim;
        }

        public async Task<User> GetUserWithPhotosAsync()
        {
            var user = await GetUserAsync();
            return await dbContext.Users.Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.Id == user.Id) 
                    ?? throw new UnauthorizedAccessException("No user is logged in");
        }

        

        public async Task<string>  GetUserId()
        {
            var user = await GetUserAsync();
            return user.Id;
        }
    }
}
