using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class CustomOrderRequest
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public string ProductType { get; set; } = null!; // Tablo, saat, pano, sandalye, vs.
        public string Dimensions { get; set; } = null!;  // 100x50 cm gibi
        public string WoodType { get; set; } = null!;    // Ceviz, zeytin, çam vb.
        public string Color { get; set; } = null!;       // Siyah, ceviz rengi vb.
        public string AdditionalNote { get; set; }       // Kullanıcının serbest metin gireceği alan
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
