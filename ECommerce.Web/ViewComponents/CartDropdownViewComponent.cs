using ECommerce.Business.Services;
using ECommerce.DTOs.Cart;
using ECommerce.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.ViewComponents
{
    public class CartDropdownViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly ICartCookieService _cartCookieService;
        private readonly ICurrentUserService _currentUserService;

        public CartDropdownViewComponent(
            IHttpContextAccessor httpContextAccessor,
            ICartService cartService,
            ICartCookieService cartCookieService,
            ICurrentUserService currentUserService)
        {
            _httpContextAccessor = httpContextAccessor;
            _cartService = cartService;
            _cartCookieService = cartCookieService;
            _currentUserService = currentUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
                  //  Stock = ci.Product.Stock
                }).ToList() ?? new List<CartItemDto>();
            }
            else
            {
                cartItems = await _cartCookieService.GetCartItemsAsync();
            }

            return View(cartItems);
        }
    }

}
