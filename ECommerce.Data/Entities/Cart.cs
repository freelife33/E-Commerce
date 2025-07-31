using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public int? CuponId { get; set; }
        public decimal DiscountAmount { get; set; } = 0;

        public Cupon Cupon { get; set; } // Navigation (isteğe bağlı)
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
