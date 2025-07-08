using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public UsersController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)!;
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.ComplateAsync();
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _unitOfWork.Users.Update(user);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id)!;
            if (user == null)
            {
                return NotFound();
            }
            _unitOfWork.Users.Remove(user);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }
    }
}
