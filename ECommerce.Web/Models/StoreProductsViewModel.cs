using ECommerce.Data.Entities;
using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;

namespace ECommerce.Web.Models
{
    public class StoreProductsViewModel
    {
        public IEnumerable<ProductListDto> Products { get; set; }
        public IEnumerable<CategoryListDto> Categories { get; set; }
        public string SearchQuery { get; set; }
        public int SelectedCategoryId { get; set; }
    }
}
