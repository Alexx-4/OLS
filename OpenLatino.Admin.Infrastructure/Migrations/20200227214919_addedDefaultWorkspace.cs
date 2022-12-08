using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class addedDefaultWorkspace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Default",
                table: "WorkSpaces",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ForAppType",
                table: "WorkSpaces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Default",
                table: "WorkSpaces");

            migrationBuilder.DropColumn(
                name: "ForAppType",
                table: "WorkSpaces");
        }
    }
}
