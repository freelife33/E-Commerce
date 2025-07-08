using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public CategoriesController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _unitOfWork!.Categories!.GetByIdAsync(id)!;
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.ComplateAsync();
            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id)!;
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }
    }
}
