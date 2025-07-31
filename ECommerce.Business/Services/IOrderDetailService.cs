using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface IOrderDetailService
    {
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
        Task AddOrderDetailAsync(OrderDetail detail);
    }

}
