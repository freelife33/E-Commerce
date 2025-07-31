using AutoMapper;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
using ECommerce.DTOs.Bank;
using ECommerce.DTOs.PaymentMethod;
using ECommerce.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Listeleme için
            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.CoverImageUrl, opt =>
                    opt.MapFrom(src => src.Images != null && src.Images.Any()
                        ? (src.Images.FirstOrDefault(i => i.IsCover) != null
                            ? src.Images.FirstOrDefault(i => i.IsCover)!.ImageUrl
                            : src.Images.FirstOrDefault()!.ImageUrl)
                        : null))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.ProductRatings != null && src.ProductRatings.Any()
                        ? Math.Round(src.ProductRatings.Average(r => r.Rating), 2)
                        : 0))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock != null ? src.Stock.Quantity : 0));

            // Detay için
            CreateMap<Product, ProductDetailDto>()
                 .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.AverageRating, opt => opt.MapFrom(src =>
                    src.ProductRatings != null && src.ProductRatings.Any()
                        ? Math.Round(src.ProductRatings.Average(r => r.Rating), 2)
                        : 0))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock != null ? src.Stock.Quantity : 0));

            // Ekleme için
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => new Stock
                {
                    Quantity = src.Stock
                }));

            // Güncelleme için
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => new Stock
                {
                    Quantity = src.Stock
                }));

            // Resimler için
            CreateMap<ProductImage, ProductImageDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<PaymentMethod, PaymentMethodDto>();
        }
    }
}
