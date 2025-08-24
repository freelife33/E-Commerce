using ECommerce.Business.Services;
using ECommerce.DTOs.CustomOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Web.Controllers
{
    public class CustomOrderController : Controller
    {
        private readonly ICustomOrderService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IMemoryCache _cache;

        public CustomOrderController(ICustomOrderService service, IWebHostEnvironment env, IMemoryCache cache)
        {
            _service = service;
            _env = env;
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Index() => View(new CreateCustomOrderRequestDto());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreateCustomOrderRequestDto model, List<IFormFile>? files)
        {
            // Basit rate-limit: IP başına 10 dk'da en fazla 3 talep
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var key = $"co:{ip}";
            var count = _cache.GetOrCreate(key, e => { e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10); return 0; });
            if (count >= 3)
            {
                TempData["Error"] = "Lütfen daha sonra tekrar deneyin.";
                return View(model);
            }
            _cache.Set(key, count + 1);

            if (!ModelState.IsValid) return View(model);

            // Upload
            var uploads = new List<UploadedFileInfo>();
            if (files != null && files.Any())
            {
                var root = Path.Combine(_env.WebRootPath, "uploads", "custom-orders");
                Directory.CreateDirectory(root);
                foreach (var f in files.Take(5)) // en fazla 5 dosya
                {
                    if (f.Length == 0) continue;
                    var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                    var name = $"{Guid.NewGuid()}{ext}";
                    var path = Path.Combine(root, name);
                    using var stream = System.IO.File.Create(path);
                    await f.CopyToAsync(stream);
                    uploads.Add(new UploadedFileInfo(f.FileName, $"/uploads/custom-orders/{name}", f.ContentType, f.Length));
                }
            }

            await _service.CreateAsync(model, uploads);

            TempData["Success"] = "Talebiniz bize ulaştı. En kısa sürede dönüş yapacağız.";
            return RedirectToAction(nameof(Thanks));
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Thanks() => View();
    }
}
