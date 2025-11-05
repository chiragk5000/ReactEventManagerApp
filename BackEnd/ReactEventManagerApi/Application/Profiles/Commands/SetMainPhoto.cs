using Application.Core;
using Application.Interfaces;
using MediatR;
using System.Net;

namespace Application.Profiles.Commands
{
    public class SetMainPhoto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string PhotoId { get; set; }
        }
        public class Handler(IAppDbContext appDbContext, IUserAcessor userAcessor, IPhotoService photoService) : IRequestHandler<Command, Result<Unit>>
        {
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await userAcessor.GetUserWithPhotosAsync();
                var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
                if (photo == null)
                {
                    return Result<Unit>.Failure("Cannot find photo", (int)HttpStatusCode.InternalServerError);

                }

                user.ImageUrl = photo.Url;
               
                var result = await appDbContext.SaveChangesAsync(cancellationToken) > 0;
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating  photo", (int)HttpStatusCode.InternalServerError);


            }
        }
    }
}
