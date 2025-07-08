using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class ProductRatingRepository : Repository<ProductRating>, IProductRatingRepository
    {
        public ProductRatingRepository(AppDbContext context) : base(context) { }
    }
}