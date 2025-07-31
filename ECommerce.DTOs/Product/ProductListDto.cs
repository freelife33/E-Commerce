using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ECommerce.DTOs.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public string? CategoryName { get; set; }          
        public string? CoverImageUrl { get; set; }
        public int Stock { get; set; }
        public int ReviewCount { get; set; }              
        public double AverageRating { get; set; }
        public int SaleCounts { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public ProductDetailDto Detail { get; set; }
        public bool HasDiscount => DiscountedPrice.HasValue && DiscountStartDate <= DateTime.Now && DiscountEndDate >= DateTime.Now;
        public decimal EffectivePrice => HasDiscount ? DiscountedPrice.Value : Price;


    }
}
