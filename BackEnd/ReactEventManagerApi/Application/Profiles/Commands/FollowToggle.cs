using Application.Core;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Commands
{
    public class FollowToggle
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string TargetUserId { get; set; }
        }
        public class Handler(IAppDbContext dbContext,IUserAcessor userAcessor) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var follower = await userAcessor.GetUserAsync();
                var target = await dbContext.Users.FindAsync(request.TargetUserId, cancellationToken);
                if (target == null)
                {
                    return Result<Unit>.Failure("Target user not found", (int)HttpStatusCode.NotFound);

                }
                var following = await dbContext.UserFollowings.FindAsync([follower.Id, target.Id], cancellationToken);
                if(following ==null)
                {
                    dbContext.UserFollowings.Add(new UserFollowing
                    {
                        FollowerId = follower.Id,
                        TargetUserId = target.Id
                    });

                }
                else
                {
                    dbContext.UserFollowings.Remove(following);
                }
                return await dbContext.SaveChangesAsync(cancellationToken) > 0
                    ? Result<Unit>.Success(Unit.Value)
                    : Result<Unit>.Failure("Error update in following", (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
