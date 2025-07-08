using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ProductReview
    {

        public int Id { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }
}
