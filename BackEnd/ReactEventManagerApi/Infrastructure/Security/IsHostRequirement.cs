using Application.Interfaces;
using Infrastructure.DbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }
    public class IsHostRequirementHandler(AppDbContext dbContext,IHttpContextAccessor httpContextAccessor, IUserAcessor userAcessor) : AuthorizationHandler<IsHostRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            // to chekc user is host of that activity 
            //var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userAcessor.GetUserAsync();
            if (user == null) return;

            var httpContext = httpContextAccessor.HttpContext;
            
            if (httpContext?.GetRouteValue("id") is not string activityId) return;

            var attendee = await dbContext.ActivityAttendees.AsNoTracking().SingleOrDefaultAsync(x => x.UserId == user.Id && x.ActivityId == activityId);
            if (attendee == null)
            {
                return;

            }
            if (attendee.IsHost)
            {
                context.Succeed(requirement);
            }
        }
    }
}
