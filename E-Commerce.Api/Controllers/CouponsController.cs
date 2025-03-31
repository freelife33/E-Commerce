using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CouponsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cupon>>> GetCoupons()
        {
            var coupons = await _unitOfWork.Cupons.GetAllAsync();
            return Ok(coupons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cupon>> GetCoupon(int id)
        {
            var coupon = await _unitOfWork.Cupons.GetByIdAsync(id)!;
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }

        [HttpPost]
        public async Task<ActionResult<Cupon>> CreateCoupon(Cupon coupon)
        {
            await _unitOfWork.Cupons.AddAsync(coupon);
            await _unitOfWork.ComplateAsync();
            return CreatedAtAction("GetCoupon", new { id = coupon.Id }, coupon);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoupon(int id, Cupon coupon)
        {
            if (id != coupon.Id)
            {
                return BadRequest();
            }
            _unitOfWork.Cupons.Update(coupon);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var coupon = await _unitOfWork.Cupons.GetByIdAsync(id)!;
            if (coupon == null)
            {
                return NotFound();
            }
            _unitOfWork.Cupons.Remove(coupon);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }
    }
}
