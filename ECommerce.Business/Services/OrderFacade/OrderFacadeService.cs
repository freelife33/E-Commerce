using AutoMapper;
using ECommerce.Business.Managers;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
using ECommerce.DTOs.Bank;
using ECommerce.DTOs.Cart;
using ECommerce.DTOs.Order;
using ECommerce.DTOs.PaymentMethod;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services.OrderFacade
{
    public class OrderFacadeService : IOrderFacadeService
    {
        private readonly ICartService _cartService;
        private readonly ICartCookieService _cartCookieService;
        private readonly ICuponService _cuponService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IAddressService _addressService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductService _productService;
        private readonly IBankService _bankService;
        private readonly IMapper _mapper;

        public OrderFacadeService(
            ICartService cartService,
            ICartCookieService cartCookieService,
            ICuponService cuponService,
            IOrderService orderService,
            IOrderDetailService orderDetailService,
            IAddressService addressService,
            ICurrentUserService currentUserService,
            IProductService productService,
            IMapper mapper,
            IPaymentService paymentService,
            IBankService bankService,
            IPaymentMethodService paymentMethodService)
        {
            _cartService = cartService;
            _cartCookieService = cartCookieService;
            _cuponService = cuponService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _addressService = addressService;
            _currentUserService = currentUserService;
            _productService = productService;
            _mapper = mapper;
            _paymentService = paymentService;
            _bankService = bankService;
            _paymentMethodService = paymentMethodService;
        }

        public async Task<PlaceOrderResult> PlaceOrderAsync(OrderInputDto dto, HttpContext http)
        {
            var userId = _currentUserService.GetUserId();
            var isGuest = !_currentUserService.IsAuthenticated();
            string? guestId = null;
            decimal subTotal = 0;

            List<CartItemDto> cartItems;

            if (isGuest)
            {
                guestId = http.Session.GetString("GuestUserId");
                if (string.IsNullOrEmpty(guestId))
                {
                    guestId = Guid.NewGuid().ToString();
                    http.Session.SetString("GuestUserId", guestId);
                }

                cartItems = await _cartCookieService.GetCartItemsAsync();

                if (!cartItems.Any())
                    return PlaceOrderResult.CreateFailure("Sepetiniz boş.");

                subTotal = cartItems.Sum(i => i.Price * i.Quantity); // ✅ indirimli fiyatları toplar
            }
            else
            {
                cartItems = null!; // sadece misafirlerde kullanılıyor
                subTotal = await _cartService.CalculateCartTotalAsync(userId);

            }

            dto.SubTotal = subTotal;

            // 🔹 Kupon kontrolü
            decimal discount = 0;
            Cupon? cupon = null;

            if (!string.IsNullOrWhiteSpace(dto.CuponCode))
            {
                cupon = await _cuponService.GetValidCuponAsync(dto.CuponCode, subTotal);

                if (cupon != null)
                {
                    var alreadyUsed = userId != null && await _cuponService.HasUserUsedCuponAsync(userId, cupon.Id);
                    if (alreadyUsed)
                        return PlaceOrderResult.CreateFailure("Bu kuponu daha önce kullandınız.");

                    discount = cupon.DiscountType switch
                    {
                        DiscountType.Percentage => subTotal * cupon.DiscountAmount / 100,
                        DiscountType.FixedAmount => cupon.DiscountAmount,
                        _ => 0
                    };
                }
            }

            // 🔹 Adres işlemleri
            int? addressId = null;

            if (isGuest)
            {
                if (string.IsNullOrWhiteSpace(dto.GuestAddress))
                    return PlaceOrderResult.CreateFailure("Lütfen geçerli bir adres giriniz.");

                var newAddress = new Address
                {
                    UserId = null,
                    FullName = dto.GuestFullName,
                    District = dto.GuestStreet,
                    City = dto.GuestCity,
                    AddressLine = dto.GuestAddress,
                    CreatedAt = DateTime.UtcNow,
                    Title = "Misafir Adresi"
                };

                addressId = await _addressService.CreateAddressAsync(newAddress);
            }
            else
            {
                if (!dto.SelectedAddressId.HasValue && !string.IsNullOrWhiteSpace(dto.NewAddressDetail))
                {
                    var newAddress = new Address
                    {
                        UserId = userId,
                        Title = dto.NewAddressTitle ?? "Yeni Adres",
                        AddressLine = dto.NewAddressDetail,
                        CreatedAt = DateTime.UtcNow
                    };

                    addressId = await _addressService.CreateAddressAsync(newAddress);
                }
                else
                {
                    addressId = dto.SelectedAddressId;
                }
            }

            if (!addressId.HasValue)
                return PlaceOrderResult.CreateFailure("Adres bilgisi eksik.");

            // 🔹 Sipariş oluştur
            var order = new Order
            {
                UserId = isGuest ? null : userId,
                AddressId = addressId,
                CuponId = cupon?.Id,
                DiscountAmount = discount,
                TotalAmount = subTotal,
                OrderDate = DateTime.UtcNow,
                Status = "Hazırlanıyor"
            };

            var newOrder = await _orderService.CreateOrderAsync(order);

            // 🔹 Sipariş detaylarını ekle
            if (isGuest)
            {
                foreach (var item in cartItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Price,
                        DiscountedPrice = null // varsa mantıksal işaret ekleyebilirsin
                    };

                    await _orderDetailService.AddOrderDetailAsync(detail);
                }
            }
            else
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId!.Value);

                if (cart == null || !cart.CartItems.Any())
                    return PlaceOrderResult.CreateFailure("Sepetiniz boş.");

                foreach (var item in cart.CartItems)
                {
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        DiscountedPrice = item.DiscountedPrice
                    };

                    await _orderDetailService.AddOrderDetailAsync(detail);
                }
            }

            if (cupon != null)
                await _cuponService.IncrementUsageAsync(cupon.Id);
                       
            return PlaceOrderResult.CreateSuccess(newOrder.Id, addressId.Value);
        }
        public async Task<OrderReviewDto> GetOrderReviewAsync(int? addressId, int orderId, HttpContext http)
        {
            bool isAuthenticated = _currentUserService.IsAuthenticated();
            Cart? cart = null;
            Address? address = null;
            decimal subTotal = 0;
            List<CartItemDto> cartItems;

            if (isAuthenticated)
            {
                int? userId = _currentUserService.GetUserId();
                if (userId == null)
                    throw new UnauthorizedAccessException();

                var order = await _orderService.GetOrderByIdAsync(orderId, userId);
                cart = await _cartService.GetCartByUserIdAsync(userId.Value);

                if (cart == null || !cart.CartItems.Any())
                    throw new Exception("Sepet boş");

                address = await _addressService.GetPrimaryOrLastUsedAddressAsync(userId.Value);

                subTotal = await _cartService.CalculateCartTotalAsync(userId);

                cart.DiscountAmount = order.DiscountAmount;
                cart.CuponId = order.CuponId;
                cart.TotalAmount = order.TotalAmount - order.DiscountAmount;

                cartItems = cart.CartItems.Select(ci => new CartItemDto
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? "",
                    ImageUrl = ci.Product?.Images.FirstOrDefault()?.ImageUrl ?? string.Empty,
                    Price = ci.UnitPrice,
                    DiscountedPrice = ci.DiscountedPrice ?? ci.UnitPrice,
                    Quantity = ci.Quantity,
                    Stock = ci.Product?.Stock?.Quantity ?? 0
                }).ToList();
            }
            else
            {
                var guestId = http.Session.GetString("GuestUserId") ?? Guid.NewGuid().ToString();
                http.Session.SetString("GuestUserId", guestId);

                var cookieCartItems = await _cartCookieService.GetCartItemsAsync();

                if (cookieCartItems == null || !cookieCartItems.Any())
                    throw new Exception("Sepet boş");

                cartItems = cookieCartItems;

                // ✅ İndirimli fiyatlar cookie'den geldiği için doğrudan kullanılabilir
                subTotal = cartItems.Sum(i => i.Price * i.Quantity);

                if (addressId != null)
                    address = await _addressService.GetAddressByIdAsync(addressId.Value);

                // Sahte cart nesnesi sadece modelde kullanılacak
                cart = new Cart
                {
                    CartItems = cookieCartItems.Select(ci => new CartItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.Price
                    }).ToList(),
                    DiscountAmount = 0,
                    CuponId = null,
                    TotalAmount = subTotal
                };
            }

            var model = new OrderReviewDto
            {
                OrderId = orderId,
                CartItems = cartItems,
                Address = _mapper.Map<AddressDto>(address),
                AddressId = address?.Id ?? 0,
                SubTotal = subTotal,
                Discount = cart.DiscountAmount,
                CuponId = cart.CuponId
            };

            return model;
        }
        public async Task<bool> ConfirmPaymentAsync(PaymentInputDto dto, HttpContext http)
        {
            var isAuthenticated = _currentUserService.IsAuthenticated();
            Order? order;

            if (isAuthenticated)
            {
                var userId = _currentUserService.GetUserId();
                order = await _orderService.GetOrderByIdAsync(dto.OrderId, userId);
            }
            else
            {
                order = await _orderService.GetOrderByIdAsync(dto.OrderId, null);
            }

            if (order == null)
                return false;

            var payment = new Payment
            {
                Amount = order.TotalAmount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = dto.PaymentMethod,
                OrderId = order.Id
            };

            await _paymentService.CreatePaymentAsync(payment);
            await _orderService.UpdateOrderStatusAsync(order.Id, "Transfer Bekleniyor");

            return true;
        }
        public async Task<CheckoutPageDto> PrepareCheckoutPageAsync(HttpContext http)
        {
            var userId = _currentUserService.GetUserId();
            var isAuthenticated = _currentUserService.IsAuthenticated();
            decimal subTotal = 0;
            List<SelectListItem> addresses = new();

            if (isAuthenticated && userId > 0)
            {
                subTotal = await _cartService.CalculateCartTotalAsync(userId);
                var addressDtos = await _addressService.GetAddressesByUserIdAsync(userId);

                addresses = addressDtos.Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Title} - {a.FullAddress}"
                }).ToList();
            }
            else
            {
                var cartItems = await _cartCookieService.GetCartItemsAsync();

                //foreach (var item in cartItems)
                //{
                //    var product = await _productService.GetProductByIdAsync(item.ProductId);
                //    if (product != null)
                //        subTotal += product.Price * item.Quantity;
                //}
                subTotal = cartItems.Sum(i => i.Price * i.Quantity);
            }

            return new CheckoutPageDto
            {
                SubTotal = subTotal,
                AddressList = addresses
            };
        }
        public async Task<List<SelectListItem>> GetUserAddressSelectListAsync()
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null || userId <= 0)
                return new List<SelectListItem>();

            var addresses = await _addressService.GetAddressesByUserIdAsync(userId);

            return addresses.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Title} - {a.FullAddress}"
            }).ToList();
        }
        public async Task<string?> GenerateOrderSuccessNumberAsync(int orderId, HttpContext http)
        {
            var isAuthenticated = _currentUserService.IsAuthenticated();
            int? userId = isAuthenticated ? _currentUserService.GetUserId() : null;


            var order = await _orderService.GetOrderByIdAsync(orderId, userId);
            if (order == null) return null;

            if (isAuthenticated)
                await _cartService.ClearCartAsync(userId!.Value);
            else
                await _cartCookieService.ClearCartAsync();

            var orderNumber = await _orderService.GenerateOrderNumberAsync(order.Id);
            return orderNumber;
        }
        public async Task<List<OrderSummaryDto>> GetUserOrdersAsync(HttpContext http)
        {
            var userId = _currentUserService.GetUserId();
            if (userId == null || userId <= 0)
                return new List<OrderSummaryDto>();

            var orders = await _orderService.GetOrdersByUserIdAsync(userId.Value);

            var viewModels = orders.Select(order => new OrderSummaryDto
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                DiscountAmount = order.DiscountAmount,
                Status = order.Status,
                AddressSummary = order.Address?.AddressLine ?? "Adres yok",
                CuponCode = order.Cupon?.Code ?? ""
            }).ToList();

            return viewModels;
        }
        public async Task<UnifiedPaymentDto> GetPaymentFormDataAsync(HttpContext http)
        {
            var banks = await _bankService.GetAllAsync();
            var methods = await _paymentMethodService.GetAllAsync();

            return new UnifiedPaymentDto
            {
                BankAccounts =_mapper.Map<List<BankAccountDto>>(banks),
                PaymentMethods =_mapper.Map<List<PaymentMethodDto>>(methods)
            };
        }



    }

}
