using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class CustomOrderAttachmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BudgetMax",
                table: "CustomOrderRequests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BudgetMin",
                table: "CustomOrderRequests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DesiredDate",
                table: "CustomOrderRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EngravingText",
                table: "CustomOrderRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Finish",
                table: "CustomOrderRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CustomOrderRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "QuoteAmount",
                table: "CustomOrderRequests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuoteNote",
                table: "CustomOrderRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuotedAt",
                table: "CustomOrderRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CustomOrderRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CustomOrderRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomOrderAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomOrderRequestId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomOrderAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomOrderAttachments_CustomOrderRequests_CustomOrderRequestId",
                        column: x => x.CustomOrderRequestId,
                        principalTable: "CustomOrderRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomOrderAttachments_CustomOrderRequestId",
                table: "CustomOrderAttachments",
                column: "CustomOrderRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomOrderAttachments");

            migrationBuilder.DropColumn(
                name: "BudgetMax",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "BudgetMin",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "DesiredDate",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "EngravingText",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "Finish",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "QuoteAmount",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "QuoteNote",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "QuotedAt",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CustomOrderRequests");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CustomOrderRequests");
        }
    }
}
