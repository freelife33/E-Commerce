using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(AppDbContext context) : base(context) { }
    }
}