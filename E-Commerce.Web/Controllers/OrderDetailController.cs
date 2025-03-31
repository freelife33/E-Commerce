using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
            return View(orderDetails);
        }

        public async Task<IActionResult> Details(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id)!;
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.OrderDetails.AddAsync(orderDetail);
                await _unitOfWork.ComplateAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetail);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id)!;
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.OrderDetails.Update(orderDetail);
                await _unitOfWork.ComplateAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderDetail);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id)!;
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id)!;
            if (orderDetail == null) return NotFound();
            _unitOfWork.OrderDetails.Remove(orderDetail);
            await _unitOfWork.ComplateAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
