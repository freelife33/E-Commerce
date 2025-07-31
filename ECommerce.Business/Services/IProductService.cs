using ECommerce.Common.Models;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IProductService
    {
        // Kullanıcı tarafı
        Task<List<ProductListDto>> GetActiveProductsAsync(ProductFilterDto filterDto);
        Task<ProductDetailDto?> GetProductDetailAsync(int id);
        Task<PagedResult<ProductListDto>> GetPagedActiveProductsAsync(ProductFilterDto filter);


        // Admin 
        // Yönetici tarafı
        Task<List<ProductListDto>> GetAllProductsAsync(ProductFilterDto filterDto);
        Task<ProductDetailDto?> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(CreateProductDto dto);
        Task<bool> UpdateProductAsync(UpdateProductDto dto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> SetCoverImageAsync(int imageId, int productId);
    }
}
