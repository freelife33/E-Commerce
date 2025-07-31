using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Address
{
    public class AddressDto
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Adres Başlığı")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Adres Detayı")]
        public string FullAddress { get; set; }
       

        [Display(Name = "Adres ")]
        public string AddressLine { get; set; }

        [Required]
        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Required]
        [Display(Name = "İlçe")]
        public string District { get; set; }

        [Phone]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }
    }
}
