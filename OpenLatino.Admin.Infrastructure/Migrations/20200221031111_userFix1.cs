using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class userFix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshTokenLifeTime",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Secret",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "GoogleId",
                table: "Clients",
                newName: "UpdateKey");

            migrationBuilder.RenameColumn(
                name: "FacebookToken",
                table: "Clients",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "FacebookId",
                table: "Clients",
                newName: "AccessKey");

            migrationBuilder.AddColumn<long>(
                name: "ExpirationDate",
                table: "Clients",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "UpdateKey",
                table: "Clients",
                newName: "GoogleId");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Clients",
                newName: "FacebookToken");

            migrationBuilder.RenameColumn(
                name: "AccessKey",
                table: "Clients",
                newName: "FacebookId");

            migrationBuilder.AddColumn<int>(
                name: "RefreshTokenLifeTime",
                table: "Clients",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Secret",
                table: "Clients",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: "1",
                column: "Secret",
                value: "bQLjjzlNZ4dkIM2fe7cHjss2J+N4IMVelrolLa1NmtI=");
        }
    }
}
