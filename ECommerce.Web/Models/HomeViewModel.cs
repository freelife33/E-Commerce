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

        public string HeroTitle { get; set; } = "El Yapımı Ahşap Ürünler";
        public string HeroSubtitle { get; set; } = "Kişiye özel tasarım, doğal ahşap ve ustalık bir arada.";
        public int? HeroProductId { get; set; }
        public string HeroImageUrl { get; set; } = "/uploads/f9411a74-1b7b-4d2c-83cc-dbc3a62ea5d6.jpg";
        public string? WorkshopVideoUrl { get; set; } = "https://www.youtube.com/embed/XXXXXXXX";

    }
}
