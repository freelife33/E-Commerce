using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.CustomOrder
{
    public class UpdateCustomOrderAdminDto
    {
        public int Status { get; set; }
        public decimal? QuoteAmount { get; set; }
        public string? QuoteNote { get; set; }
    }

}
