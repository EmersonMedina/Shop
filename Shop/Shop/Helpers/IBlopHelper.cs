using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Shop.Helpers
{
    public interface IBlopHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName);

        Task<Guid> UploadBlobAsync(byte[] file, string containerName);

        Task<Guid> UploadBlobAsync(string image, string containerName);

        Task DeleteBlobAsync(Guid id, string containerName);

    }
}
