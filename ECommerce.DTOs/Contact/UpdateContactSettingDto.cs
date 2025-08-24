using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Contact
{
    public class UpdateContactSettingDto
    {
        public string CompanyName { get; set; } = "";
        public string AddressLine { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone1 { get; set; } = "";
        public string? Phone2 { get; set; }
        public string Email { get; set; } = "";
        public string? MapEmbedUrl { get; set; }
        public string? WorkingHours { get; set; }
        public List<SocialLinkDto> SocialLinks { get; set; } = new();
    }
}
