using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class TematicLayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_dbo.StyleConfiguration",
                table: "StyleConfiguration");

            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "StyleConfiguration",
                keyColumns: new[] { "LayerId", "FilterId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.AddColumn<int>(
                name: "TematicLayerId",
                table: "StyleConfiguration",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbo.StyleConfiguration",
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId", "TematicLayerId" });

            migrationBuilder.CreateTable(
                name: "TematicLayers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TematicLayers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ServiceFunctions",
                columns: new[] { "Id", "Name", "ServiceId" },
                values: new object[] { 9, "GetTematicMap", 1 });

            migrationBuilder.InsertData(
                table: "WorkspaceFunctions",
                columns: new[] { "WorkSpaceId", "FunctionId" },
                values: new object[] { 1, 9 });

            migrationBuilder.CreateIndex(
                name: "IX_TematicLayerId",
                table: "StyleConfiguration",
                column: "TematicLayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.StyleConfiguration_dbo.TematicLayer_TematicLayerId",
                table: "StyleConfiguration",
                column: "TematicLayerId",
                principalTable: "TematicLayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo.StyleConfiguration_dbo.TematicLayer_TematicLayerId",
                table: "StyleConfiguration");

            migrationBuilder.DropTable(
                name: "TematicLayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dbo.StyleConfiguration",
                table: "StyleConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_TematicLayerId",
                table: "StyleConfiguration");

            migrationBuilder.DeleteData(
                table: "WorkspaceFunctions",
                keyColumns: new[] { "WorkSpaceId", "FunctionId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "ServiceFunctions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DropColumn(
                name: "TematicLayerId",
                table: "StyleConfiguration");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dbo.StyleConfiguration",
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId" });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId" },
                values: new object[] { 4, 2, 4 });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId" },
                values: new object[] { 4, 3, 5 });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId" },
                values: new object[] { 4, 4, 6 });
        }
    }
}
