using ECommerce.Data.Entities;
using ECommerce.DTOs.Contact;

namespace ECommerce.Web.Models.Contact
{
    public class ContactPageViewModel
    {
        public ContactSetting Settings { get; set; } = new();
        public CreateContactMessageDto Form { get; set; } = new();
    }
}
