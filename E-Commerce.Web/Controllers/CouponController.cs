using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CouponController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _unitOfWork.Cupons.GetAllAsync();
            return View(coupons);
        }

        public async Task<IActionResult> Details(int id)
        {
            var coupon = await _unitOfWork.Cupons.GetByIdAsync(id)!;
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cupon coupon)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Cupons.AddAsync(coupon);
                await _unitOfWork.ComplateAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var coupon = await _unitOfWork.Cupons.GetByIdAsync(id)!;
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cupon coupon)
        {
            if (id != coupon.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Cupons.Update(coupon);
                await _unitOfWork.ComplateAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var coupon = await _unitOfWork.Cupons.GetByIdAsync(id)!;
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupon = await _unitOfWork.Cupons.GetByIdAsync(id)!;
            if (coupon == null)
            {
                return NotFound();
            }
            _unitOfWork.Cupons.Remove(coupon);
            await _unitOfWork.ComplateAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
