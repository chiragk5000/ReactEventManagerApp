using Application.Activities.DTO;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Activities.Command
{
    public class EditActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required EditActivityDTO ActivityDto  { get; set; }
        }
        public class Handler(IAppDbContext context,IMapper mapper) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities
                    .FindAsync(request.ActivityDto.Id, cancellationToken);
                if (activity == null)
                {
                    return Result<Unit>.Failure("Activity not found", 404);
                }
                mapper.Map(request.ActivityDto, activity);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to update  the activity", 404);

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
