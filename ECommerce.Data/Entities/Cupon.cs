using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Cupon
    {
        public int Id { get; set; }
        public string Code { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public DateTime ExpiryDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumOrderAmount { get; set; }
        public int UsageCount { get; set; }
        public bool IsActive { get; set; }

    }
}
