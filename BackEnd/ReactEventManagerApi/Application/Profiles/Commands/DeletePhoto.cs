using Application.Core;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles.Commands
{
    public class DeletePhoto
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string PhotoId { get; set; }
        }
        public class Handler(IAppDbContext appDbContext,IUserAcessor userAcessor, IPhotoService photoService) : IRequestHandler<Command,Result<Unit>>
        {
            public async Task <Result<Unit>> Handle(Command request,CancellationToken cancellationToken)
            {
                var user = await userAcessor.GetUserWithPhotosAsync();
                var photo =  user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
                if (photo == null)
                {
                    return Result<Unit>.Failure("Cannot find photo", (int)HttpStatusCode.InternalServerError);
                    
                }
                if (photo.Url == user.ImageUrl)
                {
                    return Result<Unit>.Failure("Cannot delete main photo", (int)HttpStatusCode.InternalServerError);

                }
                await photoService.DeletePhoto(photo.PublicId);
                user.Photos.Remove(photo);
                var result = await appDbContext.SaveChangesAsync(cancellationToken) > 0;
                return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem deleting photo", (int)HttpStatusCode.InternalServerError);


            }
        }
    }
}
