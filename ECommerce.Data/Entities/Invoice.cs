using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
