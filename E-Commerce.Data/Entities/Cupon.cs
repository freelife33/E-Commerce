using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Entities
{
    public class Cupon
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? DiscountType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }

    }
}
