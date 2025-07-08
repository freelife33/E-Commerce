using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class ReturnRequestRepository : Repository<ReturnRequest>, IReturnRequestRepository
    {
        public ReturnRequestRepository(AppDbContext context) : base(context) { }
    }
}