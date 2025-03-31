using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Categories()
        {
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Users()
        {
            return RedirectToAction("Index", "User");
        }

        public IActionResult Orders()
        {
            return RedirectToAction("Index", "Order");
        }

        public IActionResult Coupon()
        {
            return RedirectToAction("Index", "Coupon");
        }
    }
}
