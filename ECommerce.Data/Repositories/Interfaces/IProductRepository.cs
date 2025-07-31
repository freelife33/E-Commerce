using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.DTOs.Product;

namespace ECommerce.Data.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetFilteredAsync(ProductFilterDto filterDto);
        Task<List<Product>> GetActiveFilteredAsync(ProductFilterDto filterDto);
        Task<Product> GetByIdWithStockAsync(int id);
        IQueryable<Product> GetAllQueryable();
    }
}
