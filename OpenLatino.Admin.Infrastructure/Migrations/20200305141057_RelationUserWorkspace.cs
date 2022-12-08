using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class RelationUserWorkspace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4a3e472d-f10b-4cb8-8843-93baa87f9c0b");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "WorkSpaces",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpaces_ApplicationUserId",
                table: "WorkSpaces",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaces_AspNetUsers_ApplicationUserId",
                table: "WorkSpaces",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaces_AspNetUsers_ApplicationUserId",
                table: "WorkSpaces");

            migrationBuilder.DropIndex(
                name: "IX_WorkSpaces_ApplicationUserId",
                table: "WorkSpaces");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "WorkSpaces");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "4a3e472d-f10b-4cb8-8843-93baa87f9c0b", 0, "13562aa2-f8c5-47a0-8c40-2db4ee0a1252", "ApplicationUser", null, false, false, null, null, null, "��dr��źį!%�;b'�˰؉�l��M�Z�u�WxOy��49FP�P��5۔ֆh�����z", null, false, null, false, "admin@gmail.com" });
        }
    }
}
