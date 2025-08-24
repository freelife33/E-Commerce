using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public OrderManager(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<bool> PlaceOrderAsync(int userId, int addressId, string couponCode = null)
        {
            // 1. Sepeti getir
            var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
                return false;

            // 2. Adresi kontrol et
            var address = await _unitOfWork.Address.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId)
                return false;

            // 3. Kupon varsa kontrol et
            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(couponCode))
            {
                var coupon = await _unitOfWork.Cupons.GetByCodeAsync(couponCode);
                if (coupon != null && coupon.IsActive && coupon.ExpiryDate > DateTime.Now &&
                    cart.TotalAmount >= coupon.MinimumOrderAmount)
                {
                    discountAmount = coupon.DiscountAmount;
                    coupon.UsageCount++;
                    await _unitOfWork.Cupons.UpdateAsync(coupon);
                }
            }

            // 4. Sipariş oluştur
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.TotalAmount - discountAmount,
                Address = address,
                Status = "Hazırlanıyor",
                OrderDetails = cart.CartItems.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList()
            };

            await _unitOfWork.Orders.AddAsync(order);

            // 5. Sepeti temizle
            await _unitOfWork.Carts.ClearCartAsync(userId);


            // ✅ Kalıcılaştırma
            var saved = await _unitOfWork.ComplateAsync() > 0;
            if (!saved) return false;

            // (Opsiyonel) sipariş numarası üretip kaydedin
            var orderNumber = await GenerateOrderNumberAsync(order.Id);
            // Eğer Order’da OrderNumber alanınız varsa set edin ve tekrar save edin
            // order.OrderNumber = orderNumber;
            // await _unitOfWork.ComplateAsync();

            // Kullanıcı bilgisini çekin (adresde e-posta yoksa)
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            // E-posta içeriği
            var subject = $"Siparişiniz alındı - {orderNumber}";
            var body = $@"
<b>Merhaba {(user?.FullName ?? "Müşterimiz")},</b><br/>
Siparişiniz başarıyla alındı.<br/>
Sipariş No: <b>{orderNumber}</b><br/>
Toplam: <b>{order.TotalAmount:C2}</b><br/>
Durum: <b>{order.Status}</b><br/><br/>
Teşekkür ederiz.";

            // E-posta hatası sipariş akışını bozmasın
            try
            {
                if (!string.IsNullOrWhiteSpace(user?.Email))
                    await _emailService.SendAsync(user.Email, subject, body);
            }
            catch
            {
                // Loglayın; siparişi patlatmayın
            }

            return true;
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return (await _unitOfWork.Orders.GetAllAsync(o => o.UserId == userId)).ToList();
        }

        public async Task<Order> GetOrderByIdAsync(int orderId, int? userId)
        {
            return await _unitOfWork.Orders.GetFirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId, null, null);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.ComplateAsync();
            return order;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int? userId)
        {
            var order = await _unitOfWork.Orders.GetAllAsync(
                                o => o.UserId == userId,
                                  "OrderDetails.Product,Cupon,Address");

            return order.ToList();
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Sipariş bulunamadı");

            order.Status = newStatus;
            await _unitOfWork.ComplateAsync();
        }

        public async Task<List<Order>> GetOrdersByDateAsync(DateTime? date)
        {
            var orders = await _unitOfWork.Orders.GetAllAsync(
                o => date == null || o.OrderDate.Date == date.Value.Date,
                "OrderDetails.Product,Cupon,Address");

            return orders.ToList();
        }

        public async Task<string> GenerateOrderNumberAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return "";

            var date = order.OrderDate.Date;
            var daily = await _unitOfWork.Orders.GetAllAsync(o => o.OrderDate == date && o.Id <= orderId);
            var dailyCount = daily.Count();


            return $"ORD-{date:yyyyMMdd}-{dailyCount.ToString("D5")}";
        }

    }

}
