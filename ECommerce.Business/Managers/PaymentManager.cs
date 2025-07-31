using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class PaymentManager : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _unitOfWork.Payments.AddAsync(payment);
            await _unitOfWork.ComplateAsync();
        }

        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            var payments= await _unitOfWork.Payments.GetAllAsync();
            return payments.ToList();
        }

        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _unitOfWork.Payments.GetAsync(p => p.OrderId == orderId);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            _unitOfWork.Payments.Update(payment);
            await _unitOfWork.ComplateAsync();
        }
    }

}
