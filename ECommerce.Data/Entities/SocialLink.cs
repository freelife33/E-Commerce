using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class SocialLink
    {
        public int Id { get; set; }
        public int ContactSettingId { get; set; }
        public ContactSetting ContactSetting { get; set; } = null!;
        public string Name { get; set; } = "";     // Instagram, Facebook, X, YouTube...
        public string Url { get; set; } = "";
        public string IconCss { get; set; } = "fa-brands fa-instagram"; // fontawesome class
    }
}
