using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.Auth
{
    public class UserProfileViewModel
    {
        public string FullName { get; set; }

       
        public string Email { get; set; } // sadece görüntü, değiştirilmez

        [Phone]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }

}
