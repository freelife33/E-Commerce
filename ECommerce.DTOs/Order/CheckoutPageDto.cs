using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Order
{
    public class CheckoutPageDto
    {
        public decimal SubTotal { get; set; }
        public List<SelectListItem> AddressList { get; set; } = new();
    }
}
