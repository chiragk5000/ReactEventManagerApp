﻿using Application.Activities.DTO;
using Application.Core;
using Domain.Entities;
using Infrastructure.DbContext;
using Infrastructure.DbOperations.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Command
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string Id { get; set; }
        }
        public class Handler(IUserAcessor userAcessor, AppDbContext dbContext) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await dbContext.Activities.Include(x => x.Attendees).ThenInclude(x => x.User).SingleOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
                if (activity ==null)
                {
                    return Result<Unit>.Failure("Activity not found",404);
                }
                var user = await userAcessor.GetUserAsync();
                var attendance = activity.Attendees.FirstOrDefault(x => x.UserId == user.Id);
                var isHost = activity.Attendees.Any(x => x.IsHost && x.UserId == user.Id);
                if (attendance !=null)
                {
                    if (isHost)
                        activity.IsCancelled = !activity.IsCancelled;
                    else
                        activity.Attendees.Remove(attendance);
                }
                else
                {
                    activity.Attendees.Add(new ActivityAttendee
                    {
                        UserId=user.Id,
                        ActivityId=activity.Id,
                        IsHost=false
                    });

                }
                var result = await dbContext.SaveChangesAsync(cancellationToken) > 0;
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Error occured in updating the attendance", 500);

            }
        }
    }
}
