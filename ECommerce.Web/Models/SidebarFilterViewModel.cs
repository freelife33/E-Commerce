using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;

namespace ECommerce.Web.Models
{
    public class SidebarFilterViewModel
    {
        public List<CategoryListDto> Categories { get; set; } = new();
        public int SelectedCategoryId { get; set; }

        public List<ProductListDto> TopProducts { get; set; } = new();
    }

}
