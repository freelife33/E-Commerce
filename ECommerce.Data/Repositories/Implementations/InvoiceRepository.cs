using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(AppDbContext context) : base(context) { }
    }
}