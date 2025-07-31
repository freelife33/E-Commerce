using ECommerce.DTOs.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface ICategoryService
    {
        // Kullanıcı tarafı
        Task<List<CategoryListDto>> GetActiveCategoriesAsync(CategoryFilterDto filterDto);
        Task<CategoryDetailDto?> GetCategoryDetailAsync(int id);
        // Yönetici tarafı
        Task<List<CategoryListDto>> GetAllCategoriesAsync(CategoryFilterDto filterDto);
        Task<CategoryDetailDto?> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategoryAsync(CreateCategoryDto dto);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(int id);

        Task<List<SelectListItem>> GetDropdownListAsync();
    }
}
