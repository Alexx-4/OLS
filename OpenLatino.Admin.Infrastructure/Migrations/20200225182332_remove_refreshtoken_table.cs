using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class remove_refreshtoken_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(maxLength: 50, nullable: false),
                    ExpiresUtc = table.Column<DateTime>(nullable: false),
                    IssuedUtc = table.Column<DateTime>(nullable: false),
                    ProtectedTicket = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });
        }
    }
}
