using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Wishlist
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
