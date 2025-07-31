using ECommerce.Data.Entities;

namespace ECommerce.Web.Models.Order
{
    public class PaymentPageViewModel
    {
        public int OrderId { get; set; }

        public List<BankAccount> BankAccounts { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }

        public int SelectedPaymentMethodId { get; set; } // Seçilen yöntem
    }

}
