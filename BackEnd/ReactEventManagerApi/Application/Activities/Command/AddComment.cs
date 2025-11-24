using Application.Activities.DTO;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Command
{
    public class AddComment
    {
        public class Command : IRequest<Result<CommentDTO>>
        {
            public required string Body { get; set; }
            public required string ActivityId { get; set; }
        }
        public class Handler(IAppDbContext context, IMapper mapper, IUserAcessor userAccessor) : IRequestHandler<Command, Result<CommentDTO>>
        {
            public async Task<Result<CommentDTO>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities
                    .Include(x => x.Comments)
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);

                if (activity == null)
                {
                    return Result<CommentDTO>.Failure("No acitivy found", (int)HttpStatusCode.NotFound);

                }
                var user = await userAccessor.GetUserAsync();
                var comment = new Comment { UserId = user.Id, ActivityId = activity.Id, Body = request.Body };
                activity.Comments.Add(comment);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                return result ? Result<CommentDTO>.Success(mapper.Map<CommentDTO>(comment))
                    : Result<CommentDTO>.Failure("Failed to add comment", (int)HttpStatusCode.InternalServerError);

            }
        }
    }
}
