using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class OrderDetailManager : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var result = await _unitOfWork.OrderDetails
     .GetAllWithIncludes(x => x.Product)
     .Where(od => od.OrderId == orderId)
     .ToListAsync();

            return result;
        }


        public async Task AddOrderDetailAsync(OrderDetail detail)
        {
            await _unitOfWork.OrderDetails.AddAsync(detail);
            await _unitOfWork.ComplateAsync();
        }
    }

}
