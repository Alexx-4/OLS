using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class FinxingLayersNames2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Layers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Order", "ProviderInfoId" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                table: "Layers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Order", "ProviderInfoId" },
                values: new object[] { 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Layers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Order", "ProviderInfoId" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "Layers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Order", "ProviderInfoId" },
                values: new object[] { 3, 3 });
        }
    }
}
