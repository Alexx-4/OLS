using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class NewSeedTematicMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                keyValues: new object[] { 1, 3, 5, 1 });

            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                keyValues: new object[] { 3, 6, 1, 1 });

            migrationBuilder.UpdateData(
                table: "TematicLayers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "tematicPrimaryStreet");

            migrationBuilder.InsertData(
                table: "TematicLayers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "tematicCalzadaZapata" });

            migrationBuilder.InsertData(
                table: "TematicLayers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "tematicHospital" });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                values: new object[] { 3, 6, 1, 2 });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                values: new object[] { 1, 3, 5, 3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                keyValues: new object[] { 1, 3, 5, 3 });

            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                keyValues: new object[] { 3, 6, 1, 2 });

            migrationBuilder.DeleteData(
                table: "TematicLayers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TematicLayers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                values: new object[] { 3, 6, 1, 1 });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" },
                values: new object[] { 1, 3, 5, 1 });

            migrationBuilder.UpdateData(
                table: "TematicLayers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "testingName");
        }
    }
}
