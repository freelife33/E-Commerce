using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMaintenanceModeToSystemSettings3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSettingses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsConfigured = table.Column<bool>(type: "bit", nullable: false),
                    MaintenanceEnabled = table.Column<bool>(type: "bit", nullable: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenancePlannedEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaintenanceAllowedRoles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaintenanceAllowedPaths = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettingses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSettingses");
        }
    }
}
