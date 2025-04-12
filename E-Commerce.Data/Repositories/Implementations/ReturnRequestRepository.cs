using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class ReturnRequestRepository : Repository<ReturnRequest>, IReturnRequestRepository
    {
        public ReturnRequestRepository(AppDbContext context) : base(context) { }
    }
}