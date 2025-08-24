using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Contact
{
    public class CreateContactMessageDto
    {
        [Required, StringLength(120)]
        public string FullName { get; set; } = "";
        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = "";
        [StringLength(150)]
        public string? Subject { get; set; }
        [Required, StringLength(4000)]
        public string Message { get; set; } = "";
        public string? RecaptchaToken { get; set; } // opsiyonel
    }
}
