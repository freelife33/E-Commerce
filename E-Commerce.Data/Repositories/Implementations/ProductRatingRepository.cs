using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class ProductRatingRepository : Repository<ProductRating>, IProductRatingRepository
    {
        public ProductRatingRepository(AppDbContext context) : base(context) { }
    }
}