using Application.Core;
using Application.Interfaces;
using Application.Profiles.DTOs;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MediatR;
using ReactEventManagerApi.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Profiles.Queries
{
    public class GetUserActivities
    {
        public class Query : IRequest<Result<List<UserActivityDTO>>>
        {
            public required string FilterEvents { get; set; }

            public required string UserId { get; set; }
        }

        public class Handler(IAppDbContext appDbContext ,  IMapper mapper) : IRequestHandler<Query, Result<List<UserActivityDTO>>>
        {
            public async Task<Result<List<UserActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query =  appDbContext.ActivityAttendees.Where(u => u.UserId == request.UserId).OrderBy(a => a.Activity.Date).Select(x => x.Activity).AsQueryable();
                var today = DateTime.UtcNow;

                query = request.FilterEvents
                        switch
                {
                    "past" => query.Where(a=>a.Date <= today && a.Attendees.Any(x=>x.UserId == request.UserId)),
                    "hosting" => query.Where(a => a.Attendees.Any(x => x.IsHost && x.UserId == request.UserId)),
                    _ => query.Where(a => a.Date >= today && a.Attendees.Any(x => x.UserId == request.UserId))
                    
                };
                var projectedActivities = query.ProjectTo<UserActivityDTO>(mapper.ConfigurationProvider);
                var activities = await projectedActivities.ToListAsync(cancellationToken);
                return Result<List<UserActivityDTO>>.Success(activities);


            }
        }
    }
}
