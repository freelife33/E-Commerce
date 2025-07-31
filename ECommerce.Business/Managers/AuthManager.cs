using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
using ECommerce.DTOs.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class AuthManager : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthManager(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email); // Use FirstOrDefaultAsync instead of FirstOrDefault  
            if (user == null) return null!;
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (!computedHash.SequenceEqual(user.PasswordHash)) return null!;

            var roles = user.UserRoles.Select(u => u.Role.Name);

            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Name, user.UserName),
           };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> RegisterAsync(string username, string email, string password)
        {
            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserName = username,
                Email = email,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                IsEmailConfirmed = true,
                IsPhoneNumberConfirmed = true,
                IsAdmin=false,
                IsDeleted=false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {

            var user= await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email);

            return user!;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            return user!;
        }

        public async Task UpdateUserAsync(int id, UserProfileDto model)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            user.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderDetailAsync(int orderId, int userId)
        {
            var order= await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null) return null!;
            return order;
        }


        public async Task<List<Address>> GetUserAddressesAsync(int userId)
        {
            return await _context.Address.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            var address= await _context.Address.FindAsync(addressId);

            return address!;
        }

        public async Task AddAddressAsync(int userId, AddressDto model)
        {
            var address = new Address
            {
                UserId = userId,
                Title = model.Title,
                AddressLine = model.FullAddress,
                City = model.City,
                District = model.District,
                PhoneNumber = model.PhoneNumber
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddressAsync(int userId, AddressDto model)
        {
            var address = await _context.Address.FindAsync(model.Id);
            if (address == null || address.UserId != userId) return;

            address.Title = model.Title;
            address.AddressLine = model.FullAddress;
            address.City = model.City;
            address.District = model.District;
            address.PhoneNumber = model.PhoneNumber;
            address.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }


    }
}
