using AutoMapper;
using ECommerce.Business.Services;
using ECommerce.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public AdminController(IProductService productService, IProductImageService productImageService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _productImageService = productImageService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(bool showDeleted = false)
        {
            var filter = new ProductFilterDto
            {
                ShowDeleted = showDeleted
            };
            var products = await _productService.GetAllProductsAsync(filter);
            ViewBag.ShowDeleted = showDeleted;
            return View(products);
        }

        public async Task<IActionResult> List(string? search)
        {
            var filter = new ProductFilterDto
            {
                Search = search,
                ShowDeleted = false,
                IsActive = true
            };

            var products = await _productService.GetAllProductsAsync(filter);
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetDropdownListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetDropdownListAsync();
                return View(dto);
            }
            dto.IsActive = true;
            var success = await _productService.CreateProductAsync(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Ürün kaydedilemedi.");
                ViewBag.Categories = await _categoryService.GetDropdownListAsync();
                return View(dto);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
           // var updateDto = _mapper.Map<UpdateProductDto>(product);
            ViewBag.Categories = await _categoryService.GetDropdownListAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetDropdownListAsync();
                return View(dto);
            }
            var success = await _productService.UpdateProductAsync(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Ürün güncellenemedi.");
                ViewBag.Categories = await _categoryService.GetDropdownListAsync();
                return View(dto);
            }
            return RedirectToAction("Index");
        }

        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto= _mapper.Map<ProductDetailDto>(product);
            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var success = await _productService.DeleteProductAsync(Id);
            if (!success)
            {
                ModelState.AddModelError("", "Ürün silinemedi.");
                return View();
            }
            return RedirectToAction("Index");
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductDetailAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(int productId, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("", "Lütfen bir resim seçin.");
                return RedirectToAction("Edit", new { id = productId });
            }
            var result = await _productImageService.UploadImageAsync(productId, image);
            if (result == null)
            {
                ModelState.AddModelError("", "Resim yüklenemedi.");
                return RedirectToAction("Edit", new { id = productId });
            }
            return RedirectToAction("Edit", new { id = productId });
        }

        [HttpPost]
        public async Task<IActionResult> MakeCover(int imageId, int productId)
        {
            var result = await _productService.SetCoverImageAsync(imageId, productId);
            if (!result)
                return BadRequest();

            return Ok();
        }
    }
}
