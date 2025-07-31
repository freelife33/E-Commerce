using AutoMapper;
using ECommerce.Business.Managers;
using ECommerce.Business.Services;
using ECommerce.Business.Services.OrderFacade;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Implementations;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.DTOs.Address;
using ECommerce.DTOs.Cart;
using ECommerce.DTOs.Order;
using ECommerce.DTOs.Payment;
using ECommerce.Web.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECommerce.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderFacadeService _orderFacadeService;
       
        public OrderController(
           
            IOrderFacadeService orderFacadeService)
        {
                      
            _orderFacadeService = orderFacadeService;
        }

        // GET: /Order/Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var dto = await _orderFacadeService.PrepareCheckoutPageAsync(HttpContext);

            var model = new PlaceOrderViewModel
            {
                SubTotal = dto.SubTotal,
                AddressList = dto.AddressList
            };

            return View(model);
        }



        // POST: /Order/Checkout
        [HttpPost]
        public async Task<IActionResult> Checkout(PlaceOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AddressList = await _orderFacadeService.GetUserAddressSelectListAsync();
                return View(model);
            }

            var dto = new OrderInputDto
            {
                CuponCode = model.CuponCode,
                GuestFullName = model.GuestFullName,
                GuestAddress = model.GuestAddress,
                GuestStreet = model.GuestStreet,
                GuestCity = model.GuestCity,
                GuestPostalCode = model.GuestPostalCode,
                NewAddressTitle = model.NewAddressTitle,
                NewAddressDetail = model.NewAddressDetail,
                SelectedAddressId = model.SelectedAddressId,
                SubTotal = model.SubTotal // DTO’ya eklediysen
            };

            var result = await _orderFacadeService.PlaceOrderAsync(dto, HttpContext);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Sipariş oluşturulamadı.");
                return View(model);
            }

            return RedirectToAction("Review", new { addressId = result.AddressId, orderId = result.OrderId });

        }



        [HttpGet]
        public async Task<IActionResult> Review(int? addressId, int orderId)
        {
            try
            {
                // Facade servisten DTO'yu al
                var dto = await _orderFacadeService.GetOrderReviewAsync(addressId, orderId, HttpContext);

                // DTO → ViewModel dönüşümü
                var model = new OrderReviewViewModel
                {
                    OrderId = dto.OrderId,
                    CartItems = dto.CartItems,
                    Address = dto.Address,
                    AddressId = dto.AddressId,
                    SubTotal = dto.SubTotal,
                    Discount = dto.Discount,
                    Shipping = dto.Shipping,
                    CuponId = dto.CuponId
                };

                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
            catch
            {
                return RedirectToAction("Index", "Cart");
            }
        }



        [HttpGet]
        public async Task<IActionResult> Payment(int id)
        {
            var formModel = await _orderFacadeService.GetPaymentFormDataAsync(HttpContext);
            var model = new UnifiedPaymentViewModel
            {
                OrderId = id,
                BankAccounts = formModel.BankAccounts,
                PaymentMethods = formModel.PaymentMethods
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(UnifiedPaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var formModel = await _orderFacadeService.GetPaymentFormDataAsync(HttpContext);
                model.BankAccounts = formModel.BankAccounts;
                model.PaymentMethods = formModel.PaymentMethods;

                return View(model);
            }

            var dto = new PaymentInputDto
            {
                OrderId = model.OrderId,
                PaymentMethod = model.PaymentMethod
            };

            var success = await _orderFacadeService.ConfirmPaymentAsync(dto, HttpContext);
            if (!success) return NotFound();

            return RedirectToAction("Success", new { id = model.OrderId });
        }


        // GET: /Order/Success
        [HttpGet]
        public async Task<IActionResult> Success(int id)
        {
            var orderNumber = await _orderFacadeService.GenerateOrderSuccessNumberAsync(id, HttpContext);
            if (string.IsNullOrEmpty(orderNumber))
                return NotFound();

            ViewBag.OrderNumber = orderNumber;
            return View();
        }



        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> MyOrders()
        {
            var dtoList = await _orderFacadeService.GetUserOrdersAsync(HttpContext);

            // DTO → ViewModel dönüşümü
            var viewModel = dtoList.Select(dto => new OrderSummaryViewModel
            {
                OrderId = dto.OrderId,
                OrderDate = dto.OrderDate,
                TotalAmount = dto.TotalAmount,
                DiscountAmount = dto.DiscountAmount,
                Status = dto.Status,
                AddressSummary = dto.AddressSummary,
                CuponCode = dto.CuponCode
            }).ToList();
            return View(viewModel);
        }


    }
}
