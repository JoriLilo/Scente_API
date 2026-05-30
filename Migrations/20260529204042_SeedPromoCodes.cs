using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Scente.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedPromoCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "Id", "Code", "DiscountRate", "ExpiresAt", "IsActive" },
                values: new object[,]
                {
                    { 1, "SCENTE10", 0.10m, null, true },
                    { 2, "SUMMER20", 0.20m, null, true },
                    { 3, "VIP30", 0.30m, null, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
