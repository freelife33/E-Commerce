using ECommerce.DTOs.Address;
using ECommerce.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Order
{
    public class OrderReviewDto
    {
        public List<CartItemDto> CartItems { get; set; } = new();
        public AddressDto Address { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; } = 0;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total => SubTotal - Discount + Shipping;
        public int AddressId { get; set; }
        public int? CuponId { get; set; }
        public int OrderId { get; set; }
    }
}
