using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Order
{
    public class PaymentInputDto
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
