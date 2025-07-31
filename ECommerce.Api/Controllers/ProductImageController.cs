using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageService _productImageService;

        public ProductImageController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] int productId, [FromForm] IFormFile image)
        {
            var result = await _productImageService.UploadImageAsync(productId, image);
            if (result == null)
                return BadRequest("Resim yüklenemedi veya ürün bulunamadı.");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var success = await _productImageService.DeleteImageAsync(id);
            return success ? Ok("Resim silindi.") : NotFound("Resim bulunamadı.");
        }

        [HttpPost("set-cover/{id}")]
        public async Task<IActionResult> SetCoverImage(int id)
        {
            var success = await _productImageService.SetCoverImageAsync(id);
            return success ? Ok("Kapak fotoğrafı ayarlandı.") : NotFound("Resim bulunamadı.");
        }

    }
}