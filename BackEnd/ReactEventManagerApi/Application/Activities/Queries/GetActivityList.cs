using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReactEventManagerApi.DTOs;

namespace Application.Activities.Queries
{
    public class GetActivityList
    {
        public class Query : IRequest<List<ActivityDTO>> { }
        public class Handler(IAppDbContext context,IMapper mapper, IUserAcessor userAcessor) : IRequestHandler<Query, List<ActivityDTO>>
        {
            public async Task<List<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.Activities.ProjectTo<ActivityDTO>(mapper.ConfigurationProvider, new { currentUserId = await userAcessor.GetUserId() })
                    .ToListAsync(cancellationToken);
            }
        }


    }
}
