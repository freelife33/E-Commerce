using ECommerce.Data.Entities;
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
        Task<string> LoginAsync(string username, string password);
        Task<bool> UserExistAsync(string email);
    }
}
