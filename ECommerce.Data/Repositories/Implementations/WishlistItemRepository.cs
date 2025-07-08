using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class WishlistItemRepository : Repository<WishlistItem>, IWishlistItemRepository
    {
        public WishlistItemRepository(AppDbContext context) : base(context) { }
    }
}