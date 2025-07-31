using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface ICartService
    {
        Task AddToCartAsync(string userId, int productId, int quantity = 1);
        Task RemoveFromCartAsync(string userId, int productId);
        Task<List<CartItem>> GetCartItemsAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
        Task<decimal> CalculateCartTotalAsync(int? userId);
        Task UpdateQuantityAsync(int? userId, int productId, int quantity);
        Task<Cart?> GetCartByUserIdAsync(int? userId);
        Task ClearCartAsync(int userId);
        Task<decimal> CalculateCartTotalBySessionIdAsync(string sessionId);


    }
}
