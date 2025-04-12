using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext context) : base(context) { }
    }
}