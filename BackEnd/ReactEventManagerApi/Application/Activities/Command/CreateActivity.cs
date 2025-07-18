﻿using Application.Activities.DTO;
using Application.Core;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Command
{
    public class CreateActivity
    {
        public class Command : IRequest<Result<string>>
        {
            public required CreateActivityDTO ActivityDto { get; set; }
        }
        public class Handler(AppDbContext context, IMapper mapper ): IRequestHandler<Command, Result<string>>
        {
            public async Task<Result<string>> Handle(Command request,CancellationToken cancellationToken)
            {
                var activity = mapper.Map<Activity>(request.ActivityDto);

                context.Activities.Add(activity);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<string>.Failure("Failed to create  the activity", 404);

                return Result<string>.Success(activity.Id);
            }
        }
    }

}
