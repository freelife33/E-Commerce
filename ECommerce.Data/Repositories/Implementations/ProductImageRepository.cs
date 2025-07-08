using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext context) : base(context) { }
    }
}