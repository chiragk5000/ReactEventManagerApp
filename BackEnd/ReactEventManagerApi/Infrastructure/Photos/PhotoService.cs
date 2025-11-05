using Microsoft.AspNetCore.Http;
using Application.Interfaces;
using Application.Profiles.DTOs;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName,config.Value.ApiKey,config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> DeletePhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Error !=null)
            {
                throw new Exception(result.Error.Message);

            }
            return result.Result;
        }

        public async Task<PhotoUploadResults?> UploadPhoto(IFormFile file)
        {
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation= new Transformation().Height(500).Width(500).Crop("fill"),
                    Folder = "Reactivities" + System.DateTime.UtcNow.Year
                };
                var uploadResults = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResults.Error !=null)
                {
                    throw new Exception(uploadResults.Error.Message);
                }
                return new PhotoUploadResults { PublicId = uploadResults.PublicId, Url = uploadResults.SecureUrl.AbsoluteUri };
            }
            return null;
        }
    }
}
