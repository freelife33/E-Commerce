using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Services
{
    public interface ICuponService
    {
        Task<Cupon> GetCuponByCodeAsync(string code);
        Task<bool> IsCuponValidAsync(string code, decimal orderTotal);
        Task<Cupon> GetValidCuponAsync(string code, decimal subtotal);
        Task IncrementUsageAsync(int cuponId);
        Task<bool> HasUserUsedCuponAsync(int? userId, int cuponId);

    }

}
