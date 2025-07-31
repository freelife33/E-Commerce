using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Data.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public Task<List<Product>> GetActiveFilteredAsync(ProductFilterDto filterDto)
        {
            filterDto ??= new ProductFilterDto();
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Stock)
                .Where(p => p.IsActive)
                .AsQueryable();

            //Arama ve filtreleme işlemleri
            if (!string.IsNullOrWhiteSpace(filterDto.Search))
            {
                query = query.Where(p => p.Name.Contains(filterDto.Search) || p.Description.Contains(filterDto.Search));
            }
            if (filterDto.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filterDto.CategoryId.Value);
            }
            if (filterDto.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filterDto.MinPrice.Value);
            }
            if (filterDto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filterDto.MaxPrice.Value);
            }

            //Sıralama işlemleri
            switch (filterDto.SortBy?.ToLower())
            {
                case "price":
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                case "rating":
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.ProductRatings.Count) : query.OrderBy(p => p.ProductRatings.Count);
                    break;
                default:
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
            }

            //Sayfalama işlemleri
            var skip = (filterDto.Page - 1) * filterDto.PageSize;
            return query.Skip(skip).Take(filterDto.PageSize).ToListAsync();
        }

        public IQueryable<Product> GetAllQueryable()
        {
            return _context.Products.AsQueryable();
        }

        public async Task<Product> GetByIdWithStockAsync(int id)
        {
            return await _context.Products
       .Include(p => p.Stock)
       .Include(p => p.Category)
       .Include(p => p.Images)
       .FirstOrDefaultAsync(p => p.Id == id);
        }

        public Task<List<Product>> GetFilteredAsync(ProductFilterDto filterDto)
        {
            var query = _context.Products.Include(p => p.Category).Include(p => p.Images).Include(s => s.Stock).AsQueryable();

            //Arama ve filtreleme işlemleri
            if (!string.IsNullOrWhiteSpace(filterDto.Search))
            {
                query = query.Where(p => p.Name.Contains(filterDto.Search) || p.Description.Contains(filterDto.Search));
            }
            if (filterDto.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == filterDto.CategoryId.Value);
            }
            if (filterDto.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filterDto.MinPrice.Value);
            }
            if (filterDto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filterDto.MaxPrice.Value);
            }

            //Sıralama işlemleri
            switch (filterDto.SortBy?.ToLower())
            {
                case "price":
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
                    break;
                case "rating":
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.ProductRatings.Count) : query.OrderBy(p => p.ProductRatings.Count);
                    break;
                case "Name":
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
                default:
                    query = filterDto.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                    break;
            }

            //Sayfalama işlemleri
            var skip = (filterDto.Page - 1) * filterDto.PageSize;
            return query.Skip(skip).Take(filterDto.PageSize).ToListAsync();
        }
               
    }
}
