using ECommerce.DTOs.Cart;
using ECommerce.Data.Entities;
using ECommerce.DTOs.Address;
namespace ECommerce.Web.Models.Order
{
    public class OrderReviewViewModel
    {
        public List<CartItemDto> CartItems { get; set; } = new();
        public AddressDto Address { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; } = 0; // şimdilik sıfır
        public decimal Price { get; set; } 
        public int Quantity { get; set; } 
        public decimal Total => SubTotal - Discount + Shipping;
        public int AddressId { get; set; }
        public int? CuponId { get; set; }
        public int OrderId { get; set; }
    }

}
