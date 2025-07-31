using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.ProductImage
{
    public class ProductImageDto
    {

        public int Id { get; set; }
        public string ImageUrl { get; set; }= string.Empty;
        public bool IsCover { get; set; }
    }
}
