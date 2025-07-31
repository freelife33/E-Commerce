using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCuponOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Cupons");

            migrationBuilder.AddColumn<int>(
                name: "CuponId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CuponId",
                table: "Orders",
                column: "CuponId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cupons_CuponId",
                table: "Orders",
                column: "CuponId",
                principalTable: "Cupons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cupons_CuponId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CuponId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CuponId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "DiscountType",
                table: "Cupons",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
