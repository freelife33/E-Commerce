using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.ProductImage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Business.Managers
{
    public class ProductImageService : IProductImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public ProductImageService(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var image = await _unitOfWork.ProductImages.FindAsync(i => i.Id == imageId)
            .ContinueWith(t => t.Result.FirstOrDefault());

            if (image == null)
                return false;

            var fullPath = Path.Combine(_env.WebRootPath, image.ImageUrl.Replace("/", "\\"));
            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _unitOfWork.ProductImages.Remove(image);
            await _unitOfWork.ComplateAsync();

            return true;
        }

        public async Task<bool> SetCoverImageAsync(int imageId)
        {
            var image = await _unitOfWork.ProductImages.FindAsync(i => i.Id == imageId)
            .ContinueWith(t => t.Result.FirstOrDefault());

            if (image == null)
                return false;

            var allImages = await _unitOfWork.ProductImages.FindAsync(i => i.ProductId == image.ProductId);
            foreach (var img in allImages)
                img.IsCover = (img.Id == image.Id);

            await _unitOfWork.ComplateAsync();
            return true;
        }

        public async Task<ProductImageDto?> UploadImageAsync(int productId, IFormFile image)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null || image == null || image.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var relativePath = Path.Combine("uploads", fileName).Replace("\\", "/");

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = relativePath,
                IsCover = false
            };

            await _unitOfWork.ProductImages.AddAsync(productImage);
            await _unitOfWork.ComplateAsync();

            return new ProductImageDto
            {
                Id = productImage.Id,
                ImageUrl = productImage.ImageUrl,
                IsCover = productImage.IsCover
            };
        }
    }
}
