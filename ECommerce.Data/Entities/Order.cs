using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
        public string Status { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Payment Payment { get; set; }
        public Invoice Invoice { get; set; }
        public Shipment Shipment { get; set; }
        public ICollection<ReturnRequest> ReturnRequests { get; set; }
        public ICollection<Log> Logs { get; set; }


    }
}