using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.Address
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Adres Başlığı")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Adres Detayı")]
        public string FullAddress { get; set; }

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
