using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IAddressService
    {
        Task<List<Address>> GetUserAddressesAsync(int userId);
        Task<Address> GetAddressByIdAsync(int addressId);
        Task<bool> IsAddressOwnedByUserAsync(int addressId, int userId);
        Task<List<AddressDto>> GetAddressesByUserIdAsync(int? userId);
        Task<int> CreateAddressAsync(Address address);
        Task<Address> GetPrimaryOrLastUsedAddressAsync(int? userId);

    }

}
