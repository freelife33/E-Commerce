using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class CustomOrderAttachment
    {
        public int Id { get; set; }
        public int CustomOrderRequestId { get; set; }
        public CustomOrderRequest Request { get; set; } = null!;

        public string FileName { get; set; } = "";
        public string FilePath { get; set; } = "";  // /uploads/custom-orders/abc123.webp
        public string ContentType { get; set; } = "";
        public long FileSize { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
