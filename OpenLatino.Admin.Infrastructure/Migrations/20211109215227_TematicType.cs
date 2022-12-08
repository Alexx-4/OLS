using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class TematicType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo.StyleConfiguration_dbo.Filters_FilterId",
                table: "StyleConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filters",
                table: "Filters");

            migrationBuilder.RenameTable(
                name: "Filters",
                newName: "TematicTypes");

            migrationBuilder.RenameColumn(
                name: "FilterId",
                table: "StyleConfiguration",
                newName: "TematicTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_FilterId",
                table: "StyleConfiguration",
                newName: "IX_TematicTypeId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "TematicTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TematicTypes",
                table: "TematicTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.StyleConfiguration_dbo.TematicType_TematicTypeId",
                table: "StyleConfiguration",
                column: "TematicTypeId",
                principalTable: "TematicTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dbo.StyleConfiguration_dbo.TematicType_TematicTypeId",
                table: "StyleConfiguration");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TematicTypes",
                table: "TematicTypes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "TematicTypes");

            migrationBuilder.RenameTable(
                name: "TematicTypes",
                newName: "Filters");

            migrationBuilder.RenameColumn(
                name: "TematicTypeId",
                table: "StyleConfiguration",
                newName: "FilterId");

            migrationBuilder.RenameIndex(
                name: "IX_TematicTypeId",
                table: "StyleConfiguration",
                newName: "IX_FilterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filters",
                table: "Filters",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dbo.StyleConfiguration_dbo.Filters_FilterId",
                table: "StyleConfiguration",
                column: "FilterId",
                principalTable: "Filters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
