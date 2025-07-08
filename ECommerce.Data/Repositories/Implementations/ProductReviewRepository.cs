using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;

namespace ECommerce.Data.Repositories.Implementations
{
    public class ProductReviewRepository : Repository<ProductReview>, IProductReviewRepository
    {
        public ProductReviewRepository(AppDbContext context) : base(context) { }
    }
}