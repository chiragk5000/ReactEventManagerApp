﻿using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Infrastructure.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReactEventManagerApi.DTOs;

namespace Application.Activities.Queries
{
    public class GetActivityDetails
    {
        public class Query : IRequest<Result<ActivityDTO>>
        {
            public required string Id { get; set; }
        }
        public class Handler(AppDbContext context,IMapper mapper) : IRequestHandler<Query, Result<ActivityDTO>>
        {
            public async Task<Result<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                
                // concept of eager laoding load all in include and theninclude
                //var activity = await context.Activities.Include(x => x.Attendees).ThenInclude(x => x.User).FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);
                var activity=await context.Activities.ProjectTo<ActivityDTO>(mapper.ConfigurationProvider).FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken); // using projection
                if (activity == null)
                {
                    return Result<ActivityDTO>.Failure("Activity not found", 404);
                }
                //return Result<ActivityDTO>.Success(mapper.Map<ActivityDTO>(activity));
                return Result<ActivityDTO>.Success(activity);
            }
        }
    }
}
