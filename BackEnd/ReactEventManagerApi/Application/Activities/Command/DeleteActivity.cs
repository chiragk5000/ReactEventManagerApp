using Application.Core;
using Application.Interfaces;
using MediatR;

namespace Application.Activities.Command
{
    public class DeleteActivity
    {
        public class Command : IRequest<Result<Unit>>
        {
            public required string Id { get; set; }
        }
        public class Handler(IAppDbContext context) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await context.Activities.FindAsync(request.Id, cancellationToken);
                if (activity == null)
                {
                    return Result<Unit>.Failure("Activity not found", 404);
                }
                context.Activities.Remove(activity);
                var result = await context.SaveChangesAsync(cancellationToken) > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete the activity", 404);

                return Result<Unit>.Success(Unit.Value);

            }
        }
    }
}
