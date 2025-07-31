using ECommerce.Business.Services;
using ECommerce.DTOs.User;
using ECommerce.Web.Models.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Data.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using ECommerce.Web.Models.Order;
using ECommerce.Web.Models.Address;
using ECommerce.DTOs.Address;
using ECommerce.Business.Managers;

namespace ECommerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICartCookieService _cartCookieService;
        private readonly ICartService _cartService;

        public AccountController(IAuthService authService, ICartCookieService cartCookieService, ICartService cartService)
        {
            _authService = authService;
            _cartCookieService = cartCookieService;
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _authService.UserExistAsync(model.Email))
            {
                ModelState.AddModelError("", "Bu e-posta ile zaten bir kullanıcı var.");
                return View(model);
            }

            var user = await _authService.RegisterAsync(model.UserName, model.Email, model.Password);

            // Otomatik giriş
            await SignInUser(user);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // 1. Kullanıcı doğrulama
            var token = await _authService.LoginAsync(model.Email, model.Password);
            if (token == null)
            {
                ModelState.AddModelError("", "E-posta ya da şifre hatalı.");
                return View(model);
            }

            // 2. Kullanıcı bilgilerini al
            var user = await _authService.GetUserByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            // 3. Kullanıcıyı sisteme giriş yapmış olarak işaretle
            await SignInUser(user, model.RememberMe);

            // 4. Eğer çerezde sepet varsa onu DB'ye taşı
            var cookieItems = await _cartCookieService.GetCartItemsAsync();
            if (cookieItems != null && cookieItems.Any())
            {
                foreach (var item in cookieItems)
                {
                    await _cartService.AddToCartAsync(user.Id.ToString(), item.ProductId, item.Quantity);
                }

                await _cartCookieService.ClearCartAsync();
            }


            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


        private async Task SignInUser(User user, bool rememberMe = false)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            foreach (var role in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = rememberMe,
                ExpiresUtc = rememberMe
            ? DateTimeOffset.UtcNow.AddDays(7)  // Beni hatırla: 7 gün
            : DateTimeOffset.UtcNow.AddMinutes(60) // Aksi halde 1 saat
            };


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User!.FindFirst(ClaimTypes.NameIdentifier!)!.Value);
            var user = await _authService.GetUserByIdAsync(userId);

            var model = new UserProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var user = new UserProfileDto{
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };
            await _authService.UpdateUserAsync(userId, user);

            TempData["SuccessMessage"] = "Bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }


        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _authService.GetUserByIdAsync(userId);

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var currentHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(model.CurrentPassword));

            if (!currentHash.SequenceEqual(user.PasswordHash))
            {
                ModelState.AddModelError("", "Mevcut şifre yanlış.");
                return View(model);
            }

            using var newHmac = new HMACSHA512(); // yeni salt
            user.PasswordSalt = newHmac.Key;
            user.PasswordHash = newHmac.ComputeHash(Encoding.UTF8.GetBytes(model.NewPassword));
            user.UpdatedDate = DateTime.UtcNow;

            await _authService.SaveChangesAsync(); // bunu aşağıda tanımlayacağız

            TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi.";
            return RedirectToAction("ChangePassword");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var orders = await _authService.GetOrdersByUserIdAsync(userId);

            var model = orders.Select(o => new OrderListViewModel
            {
                OrderId = o.Id,
                CreatedDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status
            }).ToList();

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var order = await _authService.GetOrderDetailAsync(id, userId);

            if (order == null)
                return NotFound();

            var model = new OrderDetailViewModel
            {
                OrderId = order.Id,
                CreatedDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.Address.AddressLine,
                Items = order.OrderDetails.Select(i => new OrderItemViewModel
                {
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            return View(model);
        }

        //-----------------Adres yönetimi-----------------

        [Authorize]
        public async Task<IActionResult> Addresses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var addresses = await _authService.GetUserAddressesAsync(userId);

            var model = addresses.Select(a => new AddressViewModel
            {
                Id = a.Id,
                Title = a.Title,
                FullAddress = a.AddressLine,
                City = a.City,
                District = a.District,
                PhoneNumber = a.PhoneNumber
            }).ToList();

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddAddress() => View();

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var address = new AddressDto
            {
                Title = model.Title,
                FullAddress = model.FullAddress,
                City = model.City,
                District = model.District,
                PhoneNumber = model.PhoneNumber
            };

            await _authService.AddAddressAsync(userId, address);

            return RedirectToAction("Addresses");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditAddress(int id)
        {
            var address = await _authService.GetAddressByIdAsync(id);
            if (address == null || address.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value))
                return NotFound();

            var model = new AddressViewModel
            {
                Id = address.Id,
                Title = address.Title,
                FullAddress = address.AddressLine,
                City = address.City,
                District = address.District,
                PhoneNumber = address.PhoneNumber
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var address = new AddressDto
            {
                Id = model.Id,
                Title = model.Title,
                FullAddress = model.FullAddress,
                City = model.City,
                District = model.District,
                PhoneNumber = model.PhoneNumber
            };
            await _authService.UpdateAddressAsync(userId, address);

            return RedirectToAction("Addresses");
        }

    }
}
