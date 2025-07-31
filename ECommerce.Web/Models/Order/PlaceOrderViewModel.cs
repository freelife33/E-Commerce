using ECommerce.DTOs.Address;
using ECommerce.DTOs.Cart;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.Order
{
    public class PlaceOrderViewModel
    {
        public decimal SubTotal { get; set; }
        public string? CuponCode { get; set; }
        public decimal Discount { get; set; }


        public List<CartItemDto>? CartItems { get; set; }
        public AddressDto? Address { get; set; }
        public int? AddressId { get; set; }
        public int? CuponId { get; set; }
        public int OrderId { get; set; }



        public int? SelectedAddressId { get; set; }
        public List<SelectListItem>? AddressList { get; set; } = new();

        public string? NewAddressTitle { get; set; }
        public string? NewAddressDetail { get; set; }


        // Guest bilgileri
        public bool? IsGuest { get; set; }
        public string? GuestName { get; set; }
        public string? GuestEmail { get; set; }
        public string? GuestPhone { get; set; }
        public string? GuestAddress { get; set; }
        public string? GuestFullName { get; set; }
        public string? GuestStreet { get; set; }
        public string? GuestCity { get; set; }
        public string? GuestPostalCode { get; set; }
        public string? GuestCountry { get; set; }

    }


}
