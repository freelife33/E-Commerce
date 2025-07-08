using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext context) : base(context) { }
    }
}