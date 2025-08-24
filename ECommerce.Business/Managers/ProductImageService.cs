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
            if (image == null || image.Length == 0) return null;

            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null) return null;

            // 1) Güvenlik: izin verilen uzantılar
            var allowedExts = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(image.FileName);
            if (!allowedExts.Contains(ext)) return null;

            // 2) WebRoot + hedef klasör: /uploads/products/{productId}/{yyyy}/{MM}/
            var today = DateTime.UtcNow;
            var targetFolder = Path.Combine(
                _env.WebRootPath,
                "uploads", "products",
                productId.ToString(),
                today.ToString("yyyy"),
                today.ToString("MM")
            );
            Directory.CreateDirectory(targetFolder);

            // 3) Anlamlı + benzersiz dosya adı (slug + kısa guid)
            var baseName = !string.IsNullOrWhiteSpace(product.Name)
    ? product.Name
    : (product.Category?.Name ?? "image");
            var slug = Slugify(baseName ?? "image");
            var shortGuid = Guid.NewGuid().ToString("N")[..8];
            var fileName = $"{slug}-{shortGuid}{ext}".ToLowerInvariant();

            var filePath = Path.Combine(targetFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await image.CopyToAsync(stream);
            }

            // 4) Uygulama içi relative url (web’de kullanılacak yol)
            var relativePath = Path.Combine(
                "uploads", "products",
                productId.ToString(),
                today.ToString("yyyy"),
                today.ToString("MM"),
                fileName
            ).Replace("\\", "/");

            // 5) İlk resimse kapak yap
            bool isFirstImage = !await _unitOfWork.ProductImages.AnyAsync(pi => pi.ProductId == productId);

            var productImage = new ProductImage
            {
                ProductId = productId,
                ImageUrl = relativePath,
                IsCover = isFirstImage
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

        // Basit slugify (Türkçe karakterleri de normalize eder)
        private static string Slugify(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "image";

            var normalized = input.Trim().ToLowerInvariant()
                .Replace("ç", "c").Replace("ğ", "g").Replace("ı", "i")
                .Replace("ö", "o").Replace("ş", "s").Replace("ü", "u");

            var sb = new StringBuilder(normalized.Length);
            foreach (var ch in normalized)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= '0' && ch <= '9')) sb.Append(ch);
                else if (char.IsWhiteSpace(ch) || ch == '-' || ch == '_') sb.Append('-');
                // diğer karakterler düşer
            }

            var slug = sb.ToString().Trim('-');
            return string.IsNullOrEmpty(slug) ? "image" : slug;
        }

    }
}
