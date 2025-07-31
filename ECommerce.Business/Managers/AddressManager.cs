using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Address;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace ECommerce.Business.Managers
{
    public class AddressManager : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Address>> GetUserAddressesAsync(int userId)
        {
            var addresses = await _unitOfWork.Address.GetAllAsync(a => a.UserId == userId);
            return addresses.ToList();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _unitOfWork.Address
                .GetByIdAsync(addressId);
        }

        public async Task<bool> IsAddressOwnedByUserAsync(int addressId, int userId)
        {
            var address = await GetAddressByIdAsync(addressId);
            return address != null && address.UserId == userId;
        }

        public async Task<List<AddressDto>> GetAddressesByUserIdAsync(int? userId)
        {
            var addresses = await _unitOfWork.Address.GetAllAsync(a => a.UserId == userId);

            return addresses.Select(a => new AddressDto
            {
                Id = a.Id,
                Title = a.Title,
                FullAddress = a.AddressLine,
                City = a.City,
                District = a.District,
                PhoneNumber = a.PhoneNumber,
            }).ToList();
        }

        public async Task<int> CreateAddressAsync(Address address)
        {
            await _unitOfWork.Address.AddAsync(address);
            await _unitOfWork.ComplateAsync();
            return address.Id;
        }

        public async Task<Address> GetPrimaryOrLastUsedAddressAsync(int? userId)
        {
            if (!userId.HasValue || userId.Value <= 0)
                return null;

            return await _unitOfWork.Address.GetFirstOrDefaultAsync(
     a => a.UserId == userId,
     orderBy: q => q.OrderByDescending(a => a.IsDefault).ThenByDescending(a => a.UpdatedDate));


        }




    }
}
