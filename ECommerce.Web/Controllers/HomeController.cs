using System.Diagnostics;
using ECommerce.Business.Managers;
using ECommerce.Business.Services;
using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWishlistService _wishlistService;
        private readonly ISystemSettingsService _settingsService;
        private readonly ICurrentUserService _currentUser;
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICategoryService categoryService, IWishlistService wishlistService, ISystemSettingsService settingsService, ICurrentUserService currentUser)
        {
            _logger = logger;
            _productService = productService;
            _categoryService = categoryService;
            _wishlistService = wishlistService;
            _settingsService = settingsService;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = _currentUser.GetUserId();
            int wishlistCount = 0;

            if (userId.HasValue && userId.Value > 0)
            {
                var items = await _wishlistService.GetWishlistItemsAsync(userId.Value);
                wishlistCount = items.Count;
            }

            var allProducts = await _productService.GetAllProductsAsync(new ProductFilterDto
            {
                IsActive = true,
                SortBy = "CreateDate",
                SortDescending = true
            });

            var categories = await _categoryService.GetActiveCategoriesAsync(new CategoryFilterDto
            {
                IsActive = true
            });

            var bestSellerProducts = allProducts
                .OrderByDescending(p => p.CreateDate)
                .Take(8)
                .ToList();

            var coverImages = allProducts.Select(p => p.CoverImageUrl).Where(url => !string.IsNullOrWhiteSpace(url)).ToList();

            // ✅ Hero resmi: aktif ürünlerden rastgele kapak resmi
            string heroImageUrl = "/images/no-image.png"; // fallback
            int? heroProductId = null;

            var heroCandidates = allProducts
    .Where(p => !string.IsNullOrWhiteSpace(p.CoverImageUrl))
    .ToList();

            if (heroCandidates.Any())
            {
                var rnd = new Random();
                var heroProduct = heroCandidates[rnd.Next(heroCandidates.Count)];
                heroImageUrl = heroProduct.CoverImageUrl!;
                heroProductId = heroProduct.Id; // ✅ ürün ID
            }

            var viewModel = new HomeViewModel
            {
                AllProducts = allProducts,
                Categories = categories,
                BestSellerProducts = bestSellerProducts,
                HeroImageUrl = heroImageUrl,
                HeroProductId = heroProductId,
                HeroTitle = "Yeni Gelen Ürünler",
                HeroSubtitle = "Özel tasarımlarımızı keşfedin"
            };

            return View(viewModel);
        }


        public async Task<PartialViewResult> CategoryIcons()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync(new CategoryFilterDto
            {
                IsActive = true,
            });
            return PartialView("~/Views/Shared/Partials/Category/_CategoryIcons.cshtml", categories);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Maintenance()
        {
            var s = await _settingsService.GetAsync();
            ViewBag.Message = s.MaintenanceMessage ?? "Sitemiz şu anda bakımda. Lütfen daha sonra tekrar deneyiniz.";
            ViewBag.PlannedEnd = s.MaintenancePlannedEnd;
            return View();
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
