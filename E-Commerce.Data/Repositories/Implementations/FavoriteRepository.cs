using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;

namespace E_Commerce.Data.Repositories.Implementations
{
    public class FavoriteRepository : Repository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context) { }
    }
}