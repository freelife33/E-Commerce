using ECommerce.Business.Services;
using ECommerce.Data.Entities;
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
    internal class AuthManager : IAuthService
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
            if (user == null) return null;
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (!computedHash.SequenceEqual(user.PasswordHash)) return null;

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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
                IsEmailConfirmed = false,
                IsPhoneNumberConfirmed = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<bool> UserExistAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
