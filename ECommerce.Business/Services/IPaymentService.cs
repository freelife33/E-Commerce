using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IPaymentService
    {
        Task CreatePaymentAsync(Payment payment);
        Task<List<Payment>> GetAllPaymentsAsync();
        Task<Payment> GetPaymentByOrderIdAsync(int orderId);
        Task UpdatePaymentAsync(Payment payment);
    }

}
