using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Order
{
    public class OrderInputDto
    {
        public decimal SubTotal { get; set; }
        public string? CuponCode { get; set; }

        public string? GuestFullName { get; set; }
        public string? GuestAddress { get; set; }
        public string? GuestStreet { get; set; }
        public string? GuestCity { get; set; }
        public string? GuestPostalCode { get; set; }

        public string? NewAddressTitle { get; set; }
        public string? NewAddressDetail { get; set; }
        public int? SelectedAddressId { get; set; }
    }
}
