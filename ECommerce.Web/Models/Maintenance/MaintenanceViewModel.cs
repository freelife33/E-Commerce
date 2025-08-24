namespace ECommerce.Web.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public bool Enabled { get; set; }
        public string? Message { get; set; }
        public DateTime? PlannedEnd { get; set; }
        public string? AllowedRoles { get; set; }
        public string? AllowedPaths { get; set; }
    }
}
