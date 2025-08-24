using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Subject { get; set; }
        public string Message { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsRead { get; set; } = false;
        public string Status { get; set; } = "New"; // New, Responded, Closed
    }
}
