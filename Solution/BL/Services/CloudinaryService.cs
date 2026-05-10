using BL.Services.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

namespace BL.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<(string Url, string PublicId)> UploadAsync(IFormFile file, EvidenceType type)
        {
            using var stream = file.OpenReadStream();

            switch (type)
            {
                case EvidenceType.Image:
                    var imageParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "images"
                    };
                    var imgResult = await _cloudinary.UploadAsync(imageParams);
                    return (imgResult.SecureUrl.ToString(), imgResult.PublicId.ToString());

                case EvidenceType.Video:
                    var videoParams = new VideoUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "videos"
                    };
                    var vidResult = await _cloudinary.UploadAsync(videoParams);
                    return (vidResult.SecureUrl.ToString(), vidResult.PublicId.ToString());

                default:
                    var rawParams = new RawUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        Folder = "documents"
                    };
                    var rawResult = await _cloudinary.UploadAsync(rawParams);
                    return (rawResult.SecureUrl.ToString(), rawResult.PublicId.ToString());
            }
        }
    }
}
