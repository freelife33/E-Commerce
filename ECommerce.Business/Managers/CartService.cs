using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Address;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartCookieService _cartCookieService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly AppDbContext _context;

        public CartService(IUnitOfWork unitOfWork, ICartCookieService cartCookieService, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _cartCookieService = cartCookieService;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }


        public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
        {
            var isAuthenticated = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            if (isAuthenticated)
            {
                // Kullanıcı giriş yaptıysa veritabanına ekle
                var cart = await _unitOfWork.Carts
                    .GetFirstOrDefaultAsync(c => c.UserId.HasValue && c.UserId.Value.ToString() == userId, null, null);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = int.TryParse(userId, out var parsedUserId) ? parsedUserId : throw new Exception("Invalid userId"),
                        CreatedDate = DateTime.Now
                    };

                    await _unitOfWork.Carts.AddAsync(cart);
                    await _unitOfWork.ComplateAsync();
                }

                var existingItem = await _unitOfWork.CartItems
                    .GetFirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId, null,null);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                    _unitOfWork.CartItems.Update(existingItem);
                }
                else
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(productId);
                    if (product == null) throw new Exception("Ürün bulunamadı");

                    var now = DateTime.Now;
                    decimal unitPrice = (product.DiscountedPrice.HasValue &&
                                         product.DiscountStartDate <= now &&
                                         product.DiscountEndDate >= now)
                                        ? product.DiscountedPrice.Value
                                        : product.Price;

                    var cartItem = new CartItem
                    {
                        UserId = int.TryParse(userId, out var parsedUserId) ? parsedUserId : null,
                        CartId = cart.Id,
                        ProductId = productId,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        AddedAt = DateTime.Now
                    };

                    await _unitOfWork.CartItems.AddAsync(cartItem);
                }

                await _unitOfWork.ComplateAsync();
            }
            else
            {
                // Kullanıcı giriş yapmadıysa cookie'de tut
                await _cartCookieService.AddToCartAsync(productId, quantity);
            }
        }


        public async Task RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await _unitOfWork.Carts
                .GetFirstOrDefaultAsync(c => c.UserId.HasValue && c.UserId.Value.ToString() == userId, null, null);

            if (cart == null) return;

            var item = await _unitOfWork.CartItems
                .GetFirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId, null, null);

            if (item != null)
            {
                _unitOfWork.CartItems.Remove(item);
                await _unitOfWork.ComplateAsync();
            }
        }

        public async Task<List<CartItem>> GetCartItemsAsync(string userId)
        {
            var cart = await _unitOfWork.Carts
                .GetFirstOrDefaultAsync(
                    c => c.UserId.HasValue && c.UserId.Value.ToString() == userId,
                    include: query => query
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                );

            return cart?.CartItems.ToList() ?? new List<CartItem>();
        }

        public async Task<int> GetCartItemCountAsync(string userId)
        {
            var cart = await _unitOfWork.Carts
                .GetFirstOrDefaultAsync(
                    c => c.UserId.HasValue && c.UserId.Value.ToString() == userId,
                    include: query => query.Include(c => c.CartItems)
                );

            return cart?.CartItems.Sum(i => i.Quantity) ?? 0;
        }

        public async Task<decimal> CalculateCartTotalAsync(int? userId)
        {
            if (!userId.HasValue || userId.Value <= 0)
                return 0;

            var items = await _context.CartItems
                .Where(c => c.UserId == userId.Value)
                .Include(c => c.Product)
                .ToListAsync();

            return items.Sum(i => i.UnitPrice * i.Quantity);
        }

        public async Task UpdateQuantityAsync(int? userId, int productId, int quantity)
        {
            var cart = await _unitOfWork.Carts.GetFirstOrDefaultAsync(c => c.UserId == userId, null, null);
            if (cart == null) return;

            var item = await _unitOfWork.CartItems.GetFirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.ProductId == productId, null, null);
            if (item != null)
            {
                item.Quantity = quantity;
                _unitOfWork.CartItems.Update(item);
                await _unitOfWork.ComplateAsync();
            }
        }

        public async Task<Cart?> GetCartByUserIdAsync(int? userId)
        {
           var cart= await _unitOfWork.Carts.GetFirstOrDefaultAsync(
    c => c.UserId == userId,
    include: q => q
        .Include(c => c.CartItems)
        .ThenInclude(ci => ci.Product)
         .ThenInclude(p => p.Images)
);

            return cart;
        }

        public async Task ClearCartAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.GetFirstOrDefaultAsync(c => c.UserId == userId,null,null);
            if (cart != null)
            {
                var items = await _unitOfWork.CartItems.GetAllAsync(ci => ci.CartId == cart.Id);
                _unitOfWork.CartItems.RemoveRange(items);
                await _unitOfWork.ComplateAsync(); // CartItem'lar silinsin diye
            }
        }

        public async Task<decimal> CalculateCartTotalBySessionIdAsync(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                return 0;

            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.SessionId == sessionId)
                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
                return 0;

            decimal total = 0;

            foreach (var item in cartItems)
            {
                total += item.Quantity * item.Product.Price;
            }

            return total;
        }

    }
}
