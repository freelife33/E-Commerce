using System.Diagnostics;
using ECommerce.Business.Managers;
using ECommerce.Business.Services;
using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWishlistService _wishlistService;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, IWishlistService wishlistService)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = 7; // Test için
            var wishlistItems = await _wishlistService.GetWishlistItemsAsync(userId);
            ViewBag.WishlistCount = wishlistItems.Count;

            var allProducts = await _productService.GetAllProductsAsync(new ProductFilterDto
            {
                IsActive = true,
                SortBy = "CreateDate", 
                SortDescending = true
            });
            var categories = await _categoryService.GetActiveCategoriesAsync(new CategoryFilterDto
            {
                IsActive = true,
            });
            var bestSellerProducts = allProducts
        .OrderByDescending(p => p.CreateDate) 
        .Take(8)
        .ToList();

            var viewModel = new HomeViewModel
            {
                AllProducts = allProducts,
                Categories = categories,
                BestSellerProducts = bestSellerProducts,

            };

            return View(viewModel);
        }
        
        public async Task<PartialViewResult> CategoryIcons()
        {
            var categories= await _categoryService.GetActiveCategoriesAsync(new CategoryFilterDto
            {
                IsActive = true,
            });
            return PartialView("~/Views/Shared/Partials/Category/_CategoryIcons.cshtml", categories);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
