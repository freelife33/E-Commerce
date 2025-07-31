using ECommerce.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = GetCurrentUserId();
            var items = await _wishlistService.GetWishlistItemsAsync(userId);
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            int userId = GetCurrentUserId();
            await _wishlistService.AddToWishlistAsync(userId, productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int productId)
        {
            int userId = GetCurrentUserId();
            await _wishlistService.RemoveFromWishlistAsync(userId, productId);
            return RedirectToAction("Index");
        }

        private int GetCurrentUserId()
        {
            // Örnek: Eğer Identity ile çalışıyorsanız
            //return int.Parse(User.FindFirst("UserId").Value);
            return 7; // Test amaçlı sabit bir kullanıcı ID'si döndürüyoruz
        }
    }
}
