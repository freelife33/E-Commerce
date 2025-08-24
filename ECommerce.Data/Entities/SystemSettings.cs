using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class SystemSettings
    {
        public int Id { get; set; }

        public bool IsConfigured { get; set; }

        // Bakım Modu
        public bool MaintenanceEnabled { get; set; } = false;
        public string? MaintenanceMessage { get; set; }
        public DateTime? MaintenancePlannedEnd { get; set; } // opsiyonel
        public string? MaintenanceAllowedRoles { get; set; } = "Admin"; // virgülle ayrılmış
        public string? MaintenanceAllowedPaths { get; set; } = "/account/login,/home/maintenance,/health";
    }
}
