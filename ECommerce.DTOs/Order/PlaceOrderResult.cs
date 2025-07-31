using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Order
{
    public class PlaceOrderResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int OrderId { get; set; }
        public int AddressId { get; set; }

        public static PlaceOrderResult CreateSuccess(int orderId, int addressId) =>
            new() { Success = true, OrderId = orderId, AddressId = addressId };

        public static PlaceOrderResult CreateFailure(string message) =>
            new() { Success = false, ErrorMessage = message };
    }


}
