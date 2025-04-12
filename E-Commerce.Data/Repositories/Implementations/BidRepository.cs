using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(AppDbContext context) : base(context) { }
    }
}