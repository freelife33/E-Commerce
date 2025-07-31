using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Kullanıcı tarafı
        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveProducts([FromQuery] ProductFilterDto filterDto)
        {
            var products = await _productService.GetActiveProductsAsync(filterDto);
            return Ok(products);
        }

        [HttpGet("active/{id}")]
        public async Task<IActionResult> GetProductDetail(int id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Admin tarafı

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductFilterDto filterDto)
        {
            var products = await _productService.GetAllProductsAsync(filterDto);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.CreateProductAsync(dto);
            return result ? Ok("Ürün başarıyla eklendi.") : BadRequest("Ürün eklenemedi.");
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productService.UpdateProductAsync(dto);
            return result ? Ok("Ürün başarıyla güncellendi.") : BadRequest("Ürün güncellenemedi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return result ? Ok("Ürün başarıyla silindi.") : BadRequest("Ürün silinemedi.");
        }
    }
}
