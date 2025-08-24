using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.CustomOrder
{
    public class CreateCustomOrderRequestDto
    {
        [Required, StringLength(120)]
        public string FullName { get; set; } = "";

        [Required, EmailAddress, StringLength(150)]
        public string Email { get; set; } = "";

        [Phone, StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(80)]
        public string? ProductType { get; set; }

        [StringLength(80)]
        public string? Dimensions { get; set; }

        [StringLength(80)]
        public string? WoodType { get; set; }

        [StringLength(40)]
        public string? Color { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; } = 1;

        [StringLength(60)]
        public string? Finish { get; set; }

        [StringLength(120)]
        public string? EngravingText { get; set; }

        [Range(0, 1_000_000)]
        public decimal? BudgetMin { get; set; }

        [Range(0, 1_000_000)]
        public decimal? BudgetMax { get; set; }

        public DateTime? DesiredDate { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }

        [StringLength(2000)]
        public string? AdditionalNote { get; set; }
    }

    // İş katmanını IFormFile'dan bağımsız tutmak için:
    public record UploadedFileInfo(string FileName, string SavedPath, string ContentType, long Size);

}
