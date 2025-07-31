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
    public class CuponManager : ICuponService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CuponManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Cupon> GetCuponByCodeAsync(string code)
        {
            return await _unitOfWork.Cupons
                .GetFirstOrDefaultAsync(
                    predicate: c => c.Code == code && c.IsActive && c.ExpiryDate > DateTime.UtcNow
                );
        }

        public async Task<bool> IsCuponValidAsync(string code, decimal orderTotal)
        {
            var cupon = await GetCuponByCodeAsync(code);
            if (cupon == null)
                return false;

            return orderTotal >= cupon.MinimumOrderAmount;
        }

        public async Task<Cupon> GetValidCuponAsync(string code, decimal subtotal)
        {
            var cupon = await _unitOfWork.Cupons.GetByCodeAsync(code);
            if (cupon == null || !cupon.IsActive || cupon.ExpiryDate < DateTime.Now || subtotal < cupon.MinimumOrderAmount)
                return null;

            return cupon;
        }

        public async Task IncrementUsageAsync(int cuponId)
        {
            var cupon = await _unitOfWork.Cupons.GetByIdAsync(cuponId);
            if (cupon == null) return;

            cupon.UsageCount++;

            // İsteğe bağlı: max kullanım sınırı varsa burada IsActive = false yapılabilir.
            // if (cupon.UsageCount >= cupon.MaxUsage) cupon.IsActive = false;

            _unitOfWork.Cupons.Update(cupon);
            await _unitOfWork.ComplateAsync();
        }

        public async Task<bool> HasUserUsedCuponAsync(int? userId, int cuponId)
        {
            return await _unitOfWork.Orders
                .AnyAsync(o => o.UserId == userId && o.CuponId == cuponId);
        }

    }
}
