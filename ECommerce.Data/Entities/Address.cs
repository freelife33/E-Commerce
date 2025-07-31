using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Neighborhood { get; set; } = null!;
        public string AddressLine { get; set; } = null!;
        public string PostalCode { get; set; }

        public bool IsDefault { get; set; } = false;
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
