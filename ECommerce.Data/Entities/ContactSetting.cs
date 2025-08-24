using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ContactSetting
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = "Şirket Adı";
        public string AddressLine { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone1 { get; set; } = "";
        public string? Phone2 { get; set; }
        public string Email { get; set; } = "";
        public string? MapEmbedUrl { get; set; } // Google Maps iframe src
        public string? WorkingHours { get; set; } // "Hafta içi 09:00-18:00; Cumartesi 10:00-16:00" gibi
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<SocialLink> SocialLinks { get; set; } = new List<SocialLink>();
    }

}
