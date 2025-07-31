using ECommerce.Data.Entities;
using ECommerce.DTOs.Bank;
using ECommerce.DTOs.PaymentMethod;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.Order
{
    public class UnifiedPaymentViewModel
    {
        // POST için gerekli olan alanlar
        [Required]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Lütfen bir ödeme yöntemi seçin.")]
        public string PaymentMethod { get; set; } = "BankTransfer";

        // GET için gerekli olan alanlar
        public List<BankAccountDto> BankAccounts { get; set; } = new();
        public List<PaymentMethodDto> PaymentMethods { get; set; } = new();
    }
}
