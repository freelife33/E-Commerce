using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
using ECommerce.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string username, string email, string password);
        Task<string> LoginAsync(string email, string password);
        Task<bool> UserExistAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int id, UserProfileDto model);
        Task SaveChangesAsync();
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> GetOrderDetailAsync(int orderId, int userId);
        Task<List<Address>> GetUserAddressesAsync(int userId);
        Task<Address> GetAddressByIdAsync(int addressId);
        Task AddAddressAsync(int userId, AddressDto model);
        Task UpdateAddressAsync(int userId, AddressDto model);



    }
}
