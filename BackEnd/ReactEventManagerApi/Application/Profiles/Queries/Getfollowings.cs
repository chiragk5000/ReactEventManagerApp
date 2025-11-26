using Application.Core;
using Application.Interfaces;
using Application.Prtofiles.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles.Queries
{
    public class Getfollowings
    {
        public class Query : IRequest<Result<List<UserProfile>>>
        {
            public required string Predicate { get; set; } = "followers";

            public required string UserId { get; set; }

        }
        public class Handler(IAppDbContext dbContext, IMapper mapper,IUserAcessor userAcessor) : IRequestHandler<Query, Result<List<UserProfile>>>
{
            public async Task<Result<List<UserProfile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<UserProfile>();
                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await dbContext.UserFollowings.Where(x => x.TargetUserId == request.UserId)
                            .Select(x => x.Follower).ProjectTo<UserProfile>(mapper.ConfigurationProvider,
                            new {currentUserId = await userAcessor.GetUserId()}).ToListAsync(cancellationToken);
                        break;
                    case "followings":
                        profiles = await dbContext.UserFollowings.Where(x => x.FollowerId == request.UserId)
                            .Select(x => x.TargetUser).ProjectTo<UserProfile>(mapper.ConfigurationProvider,
                            new { currentUserId = await userAcessor.GetUserId() }
                            ).ToListAsync(cancellationToken);
                        break;

                }
                return Result<List<UserProfile>>.Success(profiles);
            }

        }
    }
}
