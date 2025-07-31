using ECommerce.DTOs.Bank;
using ECommerce.DTOs.PaymentMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DTOs.Order
{
    public class UnifiedPaymentDto
    {
        // Kullanıcının seçtiği ödeme bilgileri
        public int OrderId { get; set; }
        public int PaymentMethod { get; set; }

        // Sayfa yeniden yüklendiğinde gösterilecek liste verileri
        public List<BankAccountDto> BankAccounts { get; set; } = new();
        public List<PaymentMethodDto> PaymentMethods { get; set; } = new();
    }
}
