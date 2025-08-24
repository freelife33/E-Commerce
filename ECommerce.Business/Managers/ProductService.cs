using AutoMapper;
using AutoMapper.QueryableExtensions;
using ECommerce.Business.Services;
using ECommerce.Common.Models;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Implementations;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Kullanıcı tarafı
        public async Task<List<ProductListDto>> GetActiveProductsAsync(ProductFilterDto filterDto)
        {
            var products = await _unitOfWork.Products.GetActiveFilteredAsync(filterDto);
            if (products == null || !products.Any())
            {
                return new List<ProductListDto>();
            }

            var productDtos = _mapper.Map<List<ProductListDto>>(products);
            return productDtos;
        }
        public async Task<ProductDetailDto?> GetProductDetailAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithStockAsync(id);
            if (product == null)
            {
                return null;
            }
            var productDto = _mapper.Map<ProductDetailDto>(product);
            return productDto;

        }




        //Yönetici tarafı
        public async Task<List<ProductListDto>> GetAllProductsAsync(ProductFilterDto filterDto)
        {
            var products = await _unitOfWork.Products.GetFilteredAsync(filterDto);
            if (!filterDto.ShowDeleted)
            {
                products = products.Where(p => p.IsActive).ToList();
            }
            return _mapper.Map<List<ProductListDto>>(products);
        }

        public async Task<ProductDetailDto?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdWithStockAsync(id);
            if (product == null)
            {
                return null;
            }
            var pdto = _mapper.Map<ProductDetailDto>(product);
            pdto.CoverImageUrl = product.Images?.FirstOrDefault(i => i.IsCover)?.ImageUrl;
            return pdto;

        }

        public async Task<bool> CreateProductAsync(CreateProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);
           
            product.IsActive = true;
            product.Sku = "TEMP"+ Guid.NewGuid().ToString();
            product.CreateDate = DateTime.UtcNow;
            if (dto.Images != null && dto.Images.Any())
            {
                product.Images = new List<ProductImage>();

                foreach (var image in dto.Images)
                {
                    // TODO: Upload işlemi - burada sadece yol simüle ediliyor
                    var fileName = Guid.NewGuid() + Path.GetExtension("image.FileName");
                    var imageUrl = Path.Combine("uploads", "products", fileName);

                    product.Images.Add(new ProductImage
                    {
                        ImageUrl = imageUrl,
                        Product = product
                    });

                    // Gerçek dosya yükleme işlemi yapılmalı
                }


            }
            

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.ComplateAsync();
            
            var categoryCode = BuildCategoryCode(dto.CategoryName);
            var sku = $"{categoryCode}-{dto.CategoryId}-{DateTime.UtcNow:yyyy}-{product.Id:D6}";
            string finalSku = sku;
            int attempt = 0;
            while (await _unitOfWork.Products.GetAllQueryable().AnyAsync(p => p.Sku == finalSku && p.Id != product.Id))
            {
                attempt++;
                finalSku = $"{sku}-{attempt}";
            }

            product.Sku = finalSku;

            return await _unitOfWork.ComplateAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null || !product.IsActive)
            {
                return false;
            }
            product.IsActive = false;
            product.IsDeleted = true;
            product.UpdateDate = DateTime.UtcNow;
            _unitOfWork.Products.Update(product);
            return await _unitOfWork.ComplateAsync() > 0;
        }


        public async Task<bool> UpdateProductAsync(UpdateProductDto dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return false;
            }
            var product = await _unitOfWork.Products.GetByIdWithStockAsync(dto.Id);
            if (product == null)
            {
                return false;
            }
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;
            product.UpdateDate = DateTime.UtcNow;

            if (product.Stock != null)
            {
                product.Stock.Quantity = dto.Stock;
            }
            else
            {
                product.Stock = new Stock
                {
                    Quantity = dto.Stock,
                    ProductId = product.Id
                };
            }


            _unitOfWork.Products.Update(product);
            return await _unitOfWork.ComplateAsync() > 0;

        }

        public async Task<bool> SetCoverImageAsync(int imageId, int productId)
        {
            var product = await _unitOfWork.Products
        .GetFirstOrDefaultAsync(p => p.Id == productId, include: x => x.Include(p => p.Images));

            if (product == null)
                return false;

            foreach (var img in product.Images)
                img.IsCover = img.Id == imageId;

            _unitOfWork.Products.Update(product);
            return await _unitOfWork.ComplateAsync() > 0;
        }

        public async Task<PagedResult<ProductListDto>> GetPagedActiveProductsAsync(ProductFilterDto filter)
        {
            var query = _unitOfWork.Products.GetAllQueryable().Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(filter.Search))
                query = query.Where(p => p.Name.Contains(filter.Search) || p.Description.Contains(filter.Search));

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            // sıralama işlemi ekle
            switch (filter.SortBy?.ToLower())
            {
                case "price":
                    query = filter.SortDescending
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                    break;
                case "name":
                    query = filter.SortDescending
                        ? query.OrderByDescending(p => p.Name)
                        : query.OrderBy(p => p.Name);
                    break;
                case "CreateDate":
                    query = filter.SortDescending
                        ? query.OrderByDescending(p => p.CreateDate)
                        : query.OrderBy(p => p.CreateDate);
                    break;
                default:
                    query = filter.SortDescending
                        ? query.OrderByDescending(p => p.CreateDate)
                        : query.OrderBy(p => p.CreateDate);
                    break;
            }

            var totalCount = await query.CountAsync();

            var pagedProducts = query
                .ProjectTo<ProductListDto>(_mapper.ConfigurationProvider)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);

            var products = await pagedProducts.ToListAsync();

            return new PagedResult<ProductListDto>
            {
                Items = products,
                TotalCount = totalCount,
                PageNumber = filter.Page,
                PageSize = filter.PageSize
            };
        }


        private static string BuildCategoryCode(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "GEN";

            // Trim + Upper
            var upper = name.Trim().ToUpperInvariant().Normalize(NormalizationForm.FormD);

            // Aksanları/işaretleri kaldır, harf-rakam dışını at
            var sb = new StringBuilder(3);
            foreach (var ch in upper)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc == UnicodeCategory.NonSpacingMark) continue; // diakritikleri at

                if (char.IsLetterOrDigit(ch))
                {
                    sb.Append(ch);
                    if (sb.Length == 3) break;
                }
            }

            var code = sb.ToString();
            if (code.Length == 0) code = "GEN";
            return code.PadRight(3, 'X'); // 3’ten kısaysa doldur
        }
    }
}
