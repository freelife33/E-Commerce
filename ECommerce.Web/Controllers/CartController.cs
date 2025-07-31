using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Cart;
using ECommerce.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICartService _cartService;
        private readonly ICartCookieService _cartCookieService;

        public CartController(
            IProductService productService,
            ICurrentUserService currentUserService,
            ICartService cartService,
            ICartCookieService cartCookieService)
        {
            _productService = productService;
            _currentUserService = currentUserService;
            _cartService = cartService;
            _cartCookieService = cartCookieService;
        }

        public async Task<IActionResult> Index()
        {
            List<CartItemDto> cartItems;

            if (_currentUserService.IsAuthenticated())
            {
                var userId = _currentUserService.GetUserId();
                var cart = await _cartService.GetCartByUserIdAsync(userId);

                cartItems = cart?.CartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    Quantity = ci.Quantity,
                    Price = ci.UnitPrice,
                    ImageUrl = ci.Product.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    //Stock = ci.Product.Stock.Quantity
                }).ToList() ?? new List<CartItemDto>();
            }
            else
            {
              
                cartItems = await _cartCookieService.GetCartItemsAsync();
            }

            return View(cartItems);
        }



        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            if (_currentUserService.IsAuthenticated())
            {
                var userId = _currentUserService.GetUserId();
                await _cartService.AddToCartAsync(userId.ToString(), productId, quantity);
            }
            else
            {
                await _cartCookieService.AddToCartAsync(productId, quantity);
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Remove(int productId)
        {
            if (_currentUserService.IsAuthenticated())
            {
                int? userId = _currentUserService.GetUserId();
                await _cartService.RemoveFromCartAsync(userId.ToString(), productId);
            }
            else
            {
                await _cartCookieService.RemoveFromCartAsync(productId);
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                TempData["StockWarning"] = "Ürün bulunamadı.";
                return RedirectToAction("Index");
            }

            if (quantity > product.Stock)
            {
                quantity = product.Stock;
                TempData["StockWarning"] = $"Stokta yalnızca {product.Stock} adet var.";
            }

            if (_currentUserService.IsAuthenticated())
            {
                var userId = _currentUserService.GetUserId();
                await _cartService.UpdateQuantityAsync(userId, productId, quantity);
            }
            else
            {
                await _cartCookieService.UpdateQuantityAsync(productId, quantity);
            }

            return RedirectToAction("Index");
        }

    }
}
