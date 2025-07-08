using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(AppDbContext context) : base(context) { }
    }
}