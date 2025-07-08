using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context) { }
    }
}