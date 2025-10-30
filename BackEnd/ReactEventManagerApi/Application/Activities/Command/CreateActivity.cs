﻿using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using Infrastructure.DbContext;
using Infrastructure.DbOperations.Interfaces;
using MediatR;

namespace Application.Activities.Command
{
    public class CreateActivity
    {
        public class Command : IRequest<Result<string>>
        {
            public required CreateActivityDTO ActivityDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper,IUserAcessor userAccessor ): IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request,CancellationToken cancellationToken)
            {
                var user = await userAccessor.GetUserAsync();


                var activity = mapper.Map<Activity>(request.ActivityDto);
                context.Activities.Add(activity);

                var attendee = new ActivityAttendee
                {
                    ActivityId = activity.Id,
                    UserId = user.Id,
                    IsHost = true
                };
                activity.Attendees.Add(attendee);

                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<string>.Failure("Failed to create  the activity", 400);

                return Result<string>.Success(activity.Id);

            }
        }
    }

}
