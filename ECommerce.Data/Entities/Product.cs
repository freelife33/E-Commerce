using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }

        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [Required, MaxLength(64)]
        public string Sku { get; set; } = null!;

        public string Barcode { get; set; }

        public Stock Stock { get; set; } = null!;

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<CartItem> CartItems { get; set; }

        public ICollection<WishlistItem> WishlistItems { get; set; }

        public ICollection<ProductReview> ProductReviews { get; set; }

        public ICollection<ProductRating> ProductRatings { get; set; }

        public ICollection<Auction> Auctions { get; set; }
    }
}
