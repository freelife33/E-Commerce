using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(AppDbContext context) : base(context) { }
    }
}