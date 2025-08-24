using ECommerce.Business.Services;
using ECommerce.DTOs.Contact;
using ECommerce.Web.Models.Contact;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService) => _contactService = contactService;
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new ContactPageViewModel
            {
                Settings = await _contactService.GetSettingsAsync(),
                Form = new CreateContactMessageDto()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactPageViewModel vm)
        {
            vm.Settings = await _contactService.GetSettingsAsync();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Lütfen formu eksiksiz ve geçerli doldurun.";
                return View(vm);
            }

            await _contactService.CreateMessageAsync(
                vm.Form,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers.UserAgent.ToString()
            );

            TempData["Success"] = "Mesajınız başarıyla iletildi. Teşekkür ederiz!";
            ModelState.Clear();
            vm.Form = new CreateContactMessageDto();
            return View(vm);
        }
    }
}
