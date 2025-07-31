using ECommerce.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services.OrderFacade
{
    public interface IOrderFacadeService
    {
        Task<PlaceOrderResult> PlaceOrderAsync(OrderInputDto dto, HttpContext httpContext);
        Task<OrderReviewDto> GetOrderReviewAsync(int? addressId, int orderId, HttpContext httpContext);
        Task<bool> ConfirmPaymentAsync(PaymentInputDto dto, HttpContext httpContext);
        Task<CheckoutPageDto> PrepareCheckoutPageAsync(HttpContext httpContext);
        Task<List<SelectListItem>> GetUserAddressSelectListAsync();
        Task<string?> GenerateOrderSuccessNumberAsync(int orderId, HttpContext httpContext);
        Task<List<OrderSummaryDto>> GetUserOrdersAsync(HttpContext httpContext);
        Task<UnifiedPaymentDto> GetPaymentFormDataAsync(HttpContext httpContext);


    }

}
