using Application.Core;
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


        public class Query : IRequest<Result<PagedList<ActivityDTO, DateTime?>>>
        {
            public required ActivityParams Params { get; set; }
        }
        public class Handler(IAppDbContext context,IMapper mapper, IUserAcessor userAcessor) : IRequestHandler<Query, Result<PagedList<ActivityDTO, DateTime?>>>
        {
            public async Task<Result<PagedList<ActivityDTO,DateTime?>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = context.Activities.OrderBy(x => x.Date).Where(x=>x.Date >= (request.Params.Cursor ?? request.Params.StartDate)).AsQueryable();
                var currentUserId = await userAcessor.GetUserId();

                if (!string.IsNullOrEmpty(request.Params.Filter))
                {
                    query = request.Params.Filter
                        switch
                    {
                        "isGoing" => query.Where(x => x.Attendees.Any(a => a.UserId == currentUserId)),
                        "isHost" => query.Where(x => x.Attendees.Any(a => a.IsHost && a.UserId == currentUserId)),
                        _ => query
                    };
                }
                var projectedActivities = query.ProjectTo<ActivityDTO>(mapper.ConfigurationProvider, new { currentUserId = currentUserId });
                            
                // pagination technique

                var actiivites = await projectedActivities.Take(request.Params.PageSize + 1).ToListAsync(cancellationToken);

                DateTime? nextCursor = null;
                if (actiivites.Count > request.Params.PageSize)
                {
                    nextCursor = actiivites.Last().Date;
                    actiivites.RemoveAt(actiivites.Count - 1);
                }

                return Result<PagedList<ActivityDTO, DateTime?>>
                        .Success(new PagedList<ActivityDTO, DateTime?> { items = actiivites, NextCursor = nextCursor });

                
                //var activities =  await context.Activities.ProjectTo<ActivityDTO>(mapper.ConfigurationProvider, new { currentUserId = await userAcessor.GetUserId() })
                //    .ToListAsync(cancellationToken);


            }
        }


    }
}
