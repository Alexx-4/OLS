using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class addedDefaultWorkspace2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LayerWorkspaces_Styles_VectorStyleId",
                table: "LayerWorkspaces");

            migrationBuilder.AlterColumn<int>(
                name: "VectorStyleId",
                table: "LayerWorkspaces",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_LayerWorkspaces_Styles_VectorStyleId",
                table: "LayerWorkspaces",
                column: "VectorStyleId",
                principalTable: "Styles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LayerWorkspaces_Styles_VectorStyleId",
                table: "LayerWorkspaces");

            migrationBuilder.AlterColumn<int>(
                name: "VectorStyleId",
                table: "LayerWorkspaces",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LayerWorkspaces_Styles_VectorStyleId",
                table: "LayerWorkspaces",
                column: "VectorStyleId",
                principalTable: "Styles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
