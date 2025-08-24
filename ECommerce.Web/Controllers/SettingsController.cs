using ECommerce.Business.Services;
using ECommerce.Web.Models.Maintenance;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ISystemSettingsService _service;
        public SettingsController(ISystemSettingsService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Maintenance()
        {
            var s = await _service.GetAsync();
            var vm = new MaintenanceViewModel
            {
                Enabled = s.MaintenanceEnabled,
                Message = s.MaintenanceMessage,
                PlannedEnd = s.MaintenancePlannedEnd,
                AllowedRoles = s.MaintenanceAllowedRoles,
                AllowedPaths = s.MaintenanceAllowedPaths
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Maintenance(MaintenanceViewModel vm)
        {
            var s = await _service.GetAsync();
            s.MaintenanceEnabled = vm.Enabled;
            s.MaintenanceMessage = vm.Message;
            s.MaintenancePlannedEnd = vm.PlannedEnd;
            s.MaintenanceAllowedRoles = vm.AllowedRoles;
            s.MaintenanceAllowedPaths = vm.AllowedPaths;

            await _service.UpdateAsync(s);
            TempData["Success"] = "Bakım ayarları güncellendi.";
            return RedirectToAction(nameof(Maintenance));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(bool enable)
        {
            await _service.ToggleMaintenanceAsync(enable);
            TempData["Success"] = enable ? "Bakım modu AKTİF." : "Bakım modu PASİF.";
            return RedirectToAction(nameof(Maintenance));
        }
    }
}
