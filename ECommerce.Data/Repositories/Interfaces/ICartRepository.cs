using ECommerce.Data.Entities;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart> {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task ClearCartAsync(int userId);
    }
}