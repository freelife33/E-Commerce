using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        public AuctionRepository(AppDbContext context) : base(context) { }
    }
}