using ECommerce.DTOs.Category;
using ECommerce.DTOs.Product;

namespace ECommerce.Web.Models
{
    public class StoreIndexViewModel
    {
        public IEnumerable<ProductListDto> Products { get; set; }
        public IEnumerable<CategoryListDto> Categories { get; set; }
        public IEnumerable<ProductListDto> TopProducts { get; set; }

        public ProductDetailDto Detail { get; set; }


        public int SelectedCategoryId { get; set; }
        public string PageTitle { get; set; }

        public int ProductCount => Products.Count();
        public string SearchQuery { get; set; }

        public string SortBy { get; set; }
        public bool SortDescending { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int PageNumber { get; set; }   
        public int PageSize { get; set; }     
        public int TotalCount { get; set; }   

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
