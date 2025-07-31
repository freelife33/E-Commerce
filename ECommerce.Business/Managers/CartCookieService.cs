using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace ECommerce.Business.Managers
{

    public class CartCookieService : ICartCookieService
    {
        private const string CookieName = "GuestCart";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductService _productService;
        protected readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CartCookieService(IHttpContextAccessor httpContextAccessor, IProductService productService, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _productService = productService;
            _context = context;
        }

        public async Task AddToCartAsync(int productId, int quantity)
        {
            var cart = await GetCartItemsAsync();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                //var product = await _unitOfWork.Products.GetByIdAsync(productId);
                var product = await _productService.GetProductByIdAsync(productId);
                if (product == null) return;

                var now = DateTime.Now;
                decimal unitPrice = (product.DiscountedPrice.HasValue &&
                                     product.DiscountStartDate <= now &&
                                     product.DiscountEndDate >= now)
                                    ? product.DiscountedPrice.Value
                                    : product.Price;



                cart.Add(new CartItemDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name!,
                    Price =unitPrice,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
        }

        public async Task<List<CartItemDto>> GetCartItemsAsync()
        {
            var cookie = _httpContextAccessor.HttpContext?.Request.Cookies[CookieName];
            if (string.IsNullOrEmpty(cookie))
                return new List<CartItemDto>();

            var decoded = HttpUtility.UrlDecode(cookie);
            var cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItemDto>>(decoded) ?? new();

            var result = new List<CartItemDto>();

            foreach (var item in cartItems)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null) continue;

                result.Add(new CartItemDto
                {
                    ProductId = product.Id,
                    ProductName = product.Name!,
                    ImageUrl = product.CoverImageUrl ?? product.Images.FirstOrDefault()?.ImageUrl,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Stock = product.Stock
                });
            }

            return result;
        }


        public Task ClearCartAsync()
        {
            var response = _httpContextAccessor.HttpContext?.Response;
            response?.Cookies.Delete(CookieName);
            return Task.CompletedTask;
        }

        private void SaveCart(List<CartItemDto> cart)
        {
            var response = _httpContextAccessor.HttpContext?.Response;
            var serialized = System.Text.Json.JsonSerializer.Serialize(cart);
            var encoded = HttpUtility.UrlEncode(serialized);

            response?.Cookies.Append(CookieName, encoded, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1),
                HttpOnly = true,
                IsEssential = true
            });
        }

        public async Task UpdateQuantityAsync(int productId, int quantity)
        {
            var cartItems = await GetCartItemsAsync();

            var item = cartItems.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cartItems);
            }
        }

        public async Task<Cart?> GetCartAsync(string guestId)
        {
            var cookieData = _httpContextAccessor.HttpContext.Request.Cookies["GuestCart"];
            if (string.IsNullOrEmpty(cookieData))
                return new Cart { CartItems = new List<CartItem>() };

            var cartItems = JsonConvert.DeserializeObject<List<GuestCartItemDto>>(HttpUtility.UrlDecode(cookieData));

            var result = new Cart
            {
                CartItems = new List<CartItem>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    result.CartItems.Add(new CartItem
                    {
                        Product = product,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
            }

            // Toplam hesaplamasını da eklersen güzel olur
            result.TotalAmount = result.CartItems.Sum(i => i.Product.Price * i.Quantity);

            return result;
        }

        public async Task RemoveFromCartAsync(int productId)
        {
            var cart = await GetCartItemsAsync();

            var itemToRemove = cart.FirstOrDefault(x => x.ProductId == productId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCart(cart);
            }
        }
               
    }

}
