using AutoMapper;
using ECommerce.Business.Services;
using ECommerce.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IProductImageService productImageService, ICategoryService categoryService, IMapper mapper)
        {
            _productService = productService;
            _productImageService = productImageService;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync(new ProductFilterDto
            {
                IsActive = true,
                PageSize = 5,
                SortBy = "CreatedDate", // varsa tarih alanı
                SortDescending = true
            });

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


        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null || !product.IsActive)
                return NotFound();

            return View(product);
        }
    }
}
