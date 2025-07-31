using ECommerce.DTOs.Cart;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;
using ECommerce.DTOs.Product;
using ECommerce.Web.Helpers;
using ECommerce.Business.Services;

namespace ECommerce.Web.ViewComponents
{
    public class CartItemCountViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICartService _cartService;
        private readonly ICartCookieService _cartCookieService;
        private readonly ICurrentUserService _currentUserService;

        public CartItemCountViewComponent(
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
            int itemCount = 0;

            if (_currentUserService.IsAuthenticated())
            {
                var userId = _currentUserService.GetUserId();
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                itemCount = cart?.CartItems.Sum(x => x.Quantity) ?? 0;
            }
            else
            {
                var cartItems = await _cartCookieService.GetCartItemsAsync();
                itemCount = cartItems.Sum(x => x.Quantity);
            }

            return View(itemCount);
        }
    }


}
