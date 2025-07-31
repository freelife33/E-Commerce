using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Category
{
    public class CategoryFilterDto
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
    }
}
