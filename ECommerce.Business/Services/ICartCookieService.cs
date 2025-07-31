using ECommerce.Data.Entities;
using ECommerce.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface ICartCookieService
    {
        Task<List<CartItemDto>> GetCartItemsAsync();
        Task AddToCartAsync(int productId, int quantity);
        Task RemoveFromCartAsync(int productId);
        Task ClearCartAsync();
        Task UpdateQuantityAsync(int productId, int quantity);
        Task<Cart?> GetCartAsync(string guestId);

    }

}
