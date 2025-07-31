using ECommerce.DTOs.ProductImage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IProductImageService
    {

        Task<ProductImageDto?> UploadImageAsync(int productId, IFormFile image);
        Task<bool> DeleteImageAsync(int imageId);
        Task<bool> SetCoverImageAsync(int imageId);
    }
}
