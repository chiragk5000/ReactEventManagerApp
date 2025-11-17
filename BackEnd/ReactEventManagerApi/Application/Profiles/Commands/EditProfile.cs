using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Application.Profiles.Commands
{
    public class EditProfile
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string DisplayName { get; set; } = string.Empty;
            public string Bio { get; set; } = string.Empty;
        }

        public class Handler(IAppDbContext appDbContext,IUserAcessor userAcessor) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userAcessor.GetUserAsync();
                user.DisplayName = request.DisplayName;
                user.Bio = request.Bio;
                var result = await appDbContext.SaveChangesAsync(cancellationToken) > 0;
                return result
                ? Result<Unit>.Success(Unit.Value)
                : Result<Unit>.Failure("Failed to update profile", (int)HttpStatusCode.InternalServerError);



            }
        }
    }

}
