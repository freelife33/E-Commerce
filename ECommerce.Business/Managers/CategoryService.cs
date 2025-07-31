using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // Kullanıcı tarafı

        public async Task<List<CategoryListDto>> GetActiveCategoriesAsync(CategoryFilterDto filterDto)
        {
            var query = await _unitOfWork.Categories
           .FindAsync(c => c.IsActive &&
               (string.IsNullOrEmpty(filterDto.SearchTerm) || c.Name.Contains(filterDto.SearchTerm)));

            return query.Select(c => new CategoryListDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive
            }).ToList();
        }

        public async Task<CategoryDetailDto?> GetCategoryDetailAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null || !category.IsActive) return null;

            return new CategoryDetailDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            };
        }


        //---------------------------------------

        // Yönetici tarafı

        public async Task<List<CategoryListDto>> GetAllCategoriesAsync(CategoryFilterDto filterDto)
        {
            var query = await _unitOfWork.Categories
             .FindAsync(c => string.IsNullOrEmpty(filterDto.SearchTerm) || c.Name.Contains(filterDto.SearchTerm));

            return query.Select(c => new CategoryListDto
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive
            }).ToList();
        }


        public async Task<CategoryDetailDto?> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return null;

            return new CategoryDetailDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            };
        }


        public async Task<bool> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };

            await _unitOfWork.Categories.AddAsync(category);
            return await _unitOfWork.ComplateAsync() > 0;
        }


        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);
            if (category == null) return false;

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;

            return await _unitOfWork.ComplateAsync() > 0;
        }


        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;

            _unitOfWork.Categories.Remove(category);
            return await _unitOfWork.ComplateAsync() > 0;
        }




        public async Task<List<SelectListItem>> GetDropdownListAsync()
        {
            var categories = await _unitOfWork.Categories.FindAsync(c => c.IsActive);
            return categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
        }

    }
}
