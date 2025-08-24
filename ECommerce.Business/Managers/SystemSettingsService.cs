using ECommerce.Business.Services;
using ECommerce.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Managers
{
    public class SystemSettingsService : ISystemSettingsService
    {
        private readonly AppDbContext _ctx;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "SystemSettingsCache";
        private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(20);

        public SystemSettingsService(AppDbContext ctx, IMemoryCache cache)
        {
            _ctx = ctx; _cache = cache;
        }

        public async Task<SystemSettings> GetAsync()
        {
            if (_cache.TryGetValue(CacheKey, out SystemSettings cached))
                return cached;

            var settings = await _ctx.SystemSettingses.AsNoTracking().OrderBy(x => x.Id).FirstOrDefaultAsync()
                          ?? new SystemSettings();

            _cache.Set(CacheKey, settings, CacheTtl);
            return settings;
        }

        public async Task UpdateAsync(SystemSettings settings)
        {
            _ctx.SystemSettingses.Update(settings);
            await _ctx.SaveChangesAsync();
            _cache.Remove(CacheKey);
        }

        public async Task ToggleMaintenanceAsync(bool enabled, string? message = null, DateTime? plannedEnd = null)
        {
            var s = await _ctx.SystemSettingses.OrderBy(x => x.Id).FirstOrDefaultAsync()
                    ?? new SystemSettings();
            s.MaintenanceEnabled = enabled;
            if (message != null) s.MaintenanceMessage = message;
            s.MaintenancePlannedEnd = plannedEnd;
            _ctx.SystemSettingses.Update(s);
            await _ctx.SaveChangesAsync();
            _cache.Remove(CacheKey);
        }
    }
}
