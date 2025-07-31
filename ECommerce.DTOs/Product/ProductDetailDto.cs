using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public int Stock { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string? CoverImageUrl { get; set; }
        public double AverageRating { get; set; }
        public List<ProductImageDto> Images { get; set; } = new();
        public bool IsActive { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public bool HasDiscount => DiscountedPrice.HasValue && DiscountStartDate <= DateTime.Now && DiscountEndDate >= DateTime.Now;
        public decimal EffectivePrice => HasDiscount ? DiscountedPrice.Value : Price;
    }
}
