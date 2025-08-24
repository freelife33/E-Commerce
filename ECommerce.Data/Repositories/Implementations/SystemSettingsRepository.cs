using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Implementations
{
    public class SystemSettingsRepository : Repository<SystemSettings>, ISystemSettingsRepository
    {
        public SystemSettingsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
