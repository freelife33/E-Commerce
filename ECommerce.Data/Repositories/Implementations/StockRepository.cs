using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        public StockRepository(AppDbContext context) : base(context) { }
    }
}