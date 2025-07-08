using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDetail>>> GetOrderDetails()
        {
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
            return Ok(orderDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetail>> GetOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id)!;
            if (orderDetail == null)
            {
                return NotFound();
            }
            return Ok(orderDetail);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDetail>> CreateOrderDetail(OrderDetail orderDetail)
        {
            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            await _unitOfWork.ComplateAsync();
            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.Id }, orderDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }
            _unitOfWork.OrderDetails.Update(orderDetail);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id)!;
            if (orderDetail == null)
            {
                return NotFound();
            }
            _unitOfWork.OrderDetails.Remove(orderDetail);
            await _unitOfWork.ComplateAsync();
            return NoContent();
        }

    }
}
