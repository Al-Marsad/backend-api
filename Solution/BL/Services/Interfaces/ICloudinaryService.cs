using DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;


namespace BL.Services.Interfaces
{
    public interface ICloudinaryService
    {
        public Task<(string Url, string PublicId)> UploadAsync(IFormFile file, EvidenceType type);

    }
}
