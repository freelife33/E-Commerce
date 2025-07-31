using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;

namespace ECommerce.Web.Models
{
    public class HomeViewModel
    {
        public List<CategoryListDto> Categories { get; set; }
        public List<ProductListDto> AllProducts { get; set; }             
        public List<ProductListDto> BestSellerProducts { get; set; }       
        public List<ProductListDto> NewProducts { get; set; }              
        public List<ProductListDto> FeaturedProducts { get; set; }         
       
        public List<ProductListDto> SliderProducts { get; set; }

    }
}
