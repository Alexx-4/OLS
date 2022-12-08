using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class addedWorkspaceFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkspaceFunctions",
                columns: table => new
                {
                    WorkSpaceId = table.Column<int>(nullable: false),
                    FunctionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.WorkspaceFunction", x => new { x.WorkSpaceId, x.FunctionId });
                    table.ForeignKey(
                        name: "FK_WorkspaceFunctions_ServiceFunctions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "ServiceFunctions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkspaceFunctions_WorkSpaces_WorkSpaceId",
                        column: x => x.WorkSpaceId,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ServiceFunctions",
                columns: new[] { "Id", "Name", "ServiceId" },
                values: new object[,]
                {
                    { 5, "DescribeLayers", 1 },
                    { 6, "SpatialQuery", 1 },
                    { 7, "AdvancedQuery", 2 },
                    { 8, "SpatialQuery", 2 }
                });

            migrationBuilder.InsertData(
                table: "WorkspaceFunctions",
                columns: new[] { "WorkSpaceId", "FunctionId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 3 },
                    { 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "WorkspaceFunctions",
                columns: new[] { "WorkSpaceId", "FunctionId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 1, 6 },
                    { 1, 7 },
                    { 1, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkspaceFunctions_FunctionId",
                table: "WorkspaceFunctions",
                column: "FunctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkspaceFunctions");

            migrationBuilder.DeleteData(
                table: "ServiceFunctions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ServiceFunctions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ServiceFunctions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ServiceFunctions",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
