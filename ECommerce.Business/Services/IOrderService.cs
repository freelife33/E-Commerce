using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IOrderService
    {
        Task<bool> PlaceOrderAsync(int userId, int addressId, string couponCode = null);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task<Order> GetOrderByIdAsync(int orderId, int? userId);
        Task<Order> CreateOrderAsync(Order order);
        Task<List<Order>> GetOrdersByUserIdAsync(int? userId);
        Task<List<Order>> GetOrdersByDateAsync(DateTime? date);
        Task UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<string> GenerateOrderNumberAsync(int orderId);




    }

}
