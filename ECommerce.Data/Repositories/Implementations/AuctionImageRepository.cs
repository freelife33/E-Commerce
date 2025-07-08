using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class AuctionImageRepository : Repository<AuctionImage>, IAuctionImageRepository
    {
        public AuctionImageRepository(AppDbContext context) : base(context) { }
    }
}