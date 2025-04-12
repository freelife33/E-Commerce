using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class AuctionImageRepository : Repository<AuctionImage>, IAuctionImageRepository
    {
        public AuctionImageRepository(AppDbContext context) : base(context) { }
    }
}