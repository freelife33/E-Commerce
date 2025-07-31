using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;
using ECommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public StoreController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string q = "", int categoryId = 0, int page = 1, string sortBy = "CreateDate", bool sortDesc = true, decimal? minPrice=null, decimal? maxPrice=null)
        {
            int pageSize = 9;

            var filterDto = new ProductFilterDto
            {
                IsActive = true,
                Search = q,
                Page = page,
                PageSize = pageSize,
                SortBy = sortBy,
                SortDescending = sortDesc,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            if (categoryId != 0)
                filterDto.CategoryId = categoryId;



            var pagedProducts = await _productService.GetPagedActiveProductsAsync(filterDto);

            var Images = await _productService.GetActiveProductsAsync(new ProductFilterDto { IsActive = true });
            var categories = await _categoryService.GetAllCategoriesAsync(new CategoryFilterDto
            {
                IsActive = true,
            });
            var topProducts = await _productService.GetAllProductsAsync(new ProductFilterDto
            {
                IsActive = true,
                SortBy = "CreateDate",
                SortDescending = true,
                PageSize = 8 
            });

            var viewModel = new StoreIndexViewModel
            {
                Products = pagedProducts.Items,
                Categories = categories,
                TopProducts = topProducts,
                SelectedCategoryId = categoryId,
                PageTitle = categoryId == 0 ? "Tüm Ürünler" : categories.FirstOrDefault(c => c.Id == categoryId)?.Name ?? "Ürünler",
                PageNumber = pagedProducts.PageNumber,
                PageSize = pagedProducts.PageSize,
                TotalCount = pagedProducts.TotalCount,
                SearchQuery = q,
                SortBy = sortBy,
                SortDescending = sortDesc
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Products(string q = "", int categoryId = 0)
        {
            var products = await _productService.GetActiveProductsAsync(new ProductFilterDto
            {
                IsActive = true,
                Search = q,
                //CategoryId = categoryId,
                SortBy = "CreateDate",
                SortDescending = true
            });

            var viewModel = new StoreProductsViewModel
            {
                Products = products,
                SearchQuery = q,
                SelectedCategoryId = categoryId,
                Categories = await _categoryService.GetActiveCategoriesAsync(new CategoryFilterDto
                {
                    IsActive = true,
                })
            };

            return View(viewModel);
        }
    }
}
