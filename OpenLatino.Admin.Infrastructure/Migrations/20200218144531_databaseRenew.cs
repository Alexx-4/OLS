using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenLatino.Admin.Infrastructure.Migrations
{
    public partial class databaseRenew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__MigrationHistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(maxLength: 150, nullable: false),
                    ContextKey = table.Column<string>(maxLength: 300, nullable: false),
                    Model = table.Column<byte[]>(nullable: false),
                    ProductVersion = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.__MigrationHistory", x => new { x.MigrationId, x.ContextKey });
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Secret = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ApplicationType = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    RefreshTokenLifeTime = table.Column<int>(nullable: false),
                    AllowedOrigin = table.Column<string>(maxLength: 100, nullable: true),
                    GoogleId = table.Column<string>(nullable: true),
                    FacebookId = table.Column<string>(nullable: true),
                    FacebookToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Function = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    LanguageName = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConnectionString = table.Column<string>(nullable: false),
                    PkField = table.Column<string>(nullable: false),
                    Table = table.Column<string>(nullable: false),
                    GeoField = table.Column<string>(nullable: false),
                    BoundingBoxField = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 50, nullable: false),
                    ClientId = table.Column<string>(maxLength: 50, nullable: false),
                    IssuedUtc = table.Column<DateTime>(nullable: false),
                    ExpiresUtc = table.Column<DateTime>(nullable: false),
                    ProtectedTicket = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Styles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EnableOutline = table.Column<bool>(nullable: false),
                    Fill = table.Column<string>(nullable: false),
                    Line = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    OutlinePen = table.Column<string>(nullable: false),
                    PointFill = table.Column<string>(nullable: false),
                    PointSize = table.Column<float>(nullable: false),
                    ImageContent = table.Column<byte[]>(nullable: true),
                    ImageRotation = table.Column<float>(nullable: false),
                    ImageScale = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Styles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSpaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Layers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProviderInfoId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.Layers_dbo.ProviderInfoes_ProviderInfoId",
                        column: x => x.ProviderInfoId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderTranslations",
                columns: table => new
                {
                    LanguageID = table.Column<int>(nullable: false),
                    EntityID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ProviderTranslations", x => new { x.LanguageID, x.EntityID });
                    table.ForeignKey(
                        name: "FK_dbo.ProviderTranslations_dbo.ProviderInfoes_EntityID",
                        column: x => x.EntityID,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.ProviderTranslations_dbo.Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceFunctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServiceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dbo.ServiceFunctions_dbo.Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientWorkSpaces",
                columns: table => new
                {
                    Client_Id = table.Column<string>(nullable: false),
                    WorkSpace_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.ClientWorkSpaces", x => new { x.Client_Id, x.WorkSpace_Id });
                    table.ForeignKey(
                        name: "FK_dbo.ClientWorkSpaces_dbo.Client_Client_Id",
                        column: x => x.Client_Id,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.ClientWorkSpaces_dbo.WorkSpaces_WorkSpace_Id",
                        column: x => x.WorkSpace_Id,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlfaInfos",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LayerID = table.Column<int>(nullable: false),
                    PkField = table.Column<string>(nullable: false),
                    Table = table.Column<string>(nullable: false),
                    ConnectionString = table.Column<string>(nullable: false),
                    Columns = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlfaInfos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_dbo.AlfaInfoes_dbo.Layers_LayerID",
                        column: x => x.LayerID,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LayerStyles",
                columns: table => new
                {
                    LayerId = table.Column<int>(nullable: false),
                    VectorStyleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.LayerStyles", x => new { x.LayerId, x.VectorStyleId });
                    table.ForeignKey(
                        name: "FK_LayerStyles_Layers_LayerId",
                        column: x => x.LayerId,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LayerStyles_Styles_VectorStyleId",
                        column: x => x.VectorStyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LayerTranslations",
                columns: table => new
                {
                    LanguageID = table.Column<int>(nullable: false),
                    EntityID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.LayerTranslations", x => new { x.LanguageID, x.EntityID });
                    table.ForeignKey(
                        name: "FK_dbo.LayerTranslations_dbo.Layers_EntityID",
                        column: x => x.EntityID,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.LayerTranslations_dbo.Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LayerWorkspaces",
                columns: table => new
                {
                    WorkSpace_Id = table.Column<int>(nullable: false),
                    Layer_Id = table.Column<int>(nullable: false),
                    VectorStyleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.LayerWorkSpaces", x => new { x.Layer_Id, x.WorkSpace_Id });
                    table.ForeignKey(
                        name: "FK_dbo.LayerWorkSpaces_dbo.Layer_Layer_Id",
                        column: x => x.Layer_Id,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LayerWorkspaces_Styles_VectorStyleId",
                        column: x => x.VectorStyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.LayerWorkSpaces_dbo.WorkSpaces_WorkSpace_Id",
                        column: x => x.WorkSpace_Id,
                        principalTable: "WorkSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StyleConfiguration",
                columns: table => new
                {
                    LayerId = table.Column<int>(nullable: false),
                    FilterId = table.Column<int>(nullable: false),
                    StyleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.StyleConfiguration", x => new { x.LayerId, x.FilterId });
                    table.ForeignKey(
                        name: "FK_dbo.StyleConfiguration_dbo.Filters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.StyleConfiguration_dbo.Layers_LayerId",
                        column: x => x.LayerId,
                        principalTable: "Layers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.StyleConfiguration_dbo.VectorStyles_StyleId",
                        column: x => x.StyleId,
                        principalTable: "Styles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlfaInfoTranslations",
                columns: table => new
                {
                    LanguageID = table.Column<int>(nullable: false),
                    EntityID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbo.AlfaInfoTranslations", x => new { x.LanguageID, x.EntityID });
                    table.ForeignKey(
                        name: "FK_dbo.AlfaInfoTranslations_dbo.AlfaInfoes_EntityID",
                        column: x => x.EntityID,
                        principalTable: "AlfaInfos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dbo.AlfaInfoTranslations_dbo.Languages_LanguageID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Active", "AllowedOrigin", "ApplicationType", "FacebookId", "FacebookToken", "GoogleId", "Name", "RefreshTokenLifeTime", "Secret" },
                values: new object[] { "1", false, null, 0, null, null, null, "fmujica", 0, "bQLjjzlNZ4dkIM2fe7cHjss2J+N4IMVelrolLa1NmtI=" });

            migrationBuilder.InsertData(
                table: "Filters",
                columns: new[] { "Id", "Function", "Name" },
                values: new object[,]
                {
                    { 1, new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 82, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 54, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 71, 101, 111, 109, 101, 116, 114, 121, 70, 105, 108, 116, 101, 114, 3, 0, 0, 0, 11, 95, 101, 120, 112, 114, 101, 115, 115, 105, 111, 110, 11, 95, 99, 108, 97, 117, 115, 101, 73, 110, 102, 111, 8, 95, 98, 111, 111, 108, 101, 97, 110, 4, 4, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 70, 105, 108, 116, 101, 114, 2, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 2, 0, 0, 0, 1, 2, 0, 0, 0, 10, 10, 1, 11 }, "default" },
                    { 2, new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 82, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 54, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 71, 101, 111, 109, 101, 116, 114, 121, 70, 105, 108, 116, 101, 114, 3, 0, 0, 0, 11, 95, 101, 120, 112, 114, 101, 115, 115, 105, 111, 110, 11, 95, 99, 108, 97, 117, 115, 101, 73, 110, 102, 111, 8, 95, 98, 111, 111, 108, 101, 97, 110, 4, 4, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 70, 105, 108, 116, 101, 114, 2, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 2, 0, 0, 0, 1, 2, 0, 0, 0, 10, 9, 3, 0, 0, 0, 0, 5, 3, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 4, 0, 0, 0, 23, 60, 83, 111, 117, 114, 99, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 21, 60, 78, 97, 109, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 25, 60, 79, 112, 101, 114, 97, 116, 111, 114, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 22, 60, 86, 97, 108, 117, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 4, 1, 4, 2, 56, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 73, 110, 102, 111, 83, 111, 117, 114, 99, 101, 2, 0, 0, 0, 64, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 67, 111, 109, 112, 97, 114, 105, 115, 111, 110, 79, 112, 101, 114, 97, 116, 111, 114, 2, 0, 0, 0, 2, 0, 0, 0, 5, 252, 255, 255, 255, 56, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 73, 110, 102, 111, 83, 111, 117, 114, 99, 101, 1, 0, 0, 0, 7, 118, 97, 108, 117, 101, 95, 95, 0, 8, 2, 0, 0, 0, 0, 0, 0, 0, 6, 5, 0, 0, 0, 4, 116, 121, 112, 101, 5, 250, 255, 255, 255, 64, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 67, 111, 109, 112, 97, 114, 105, 115, 111, 110, 79, 112, 101, 114, 97, 116, 111, 114, 1, 0, 0, 0, 7, 118, 97, 108, 117, 101, 95, 95, 0, 8, 2, 0, 0, 0, 6, 0, 0, 0, 6, 7, 0, 0, 0, 6, 115, 99, 104, 111, 111, 108, 11 }, "school" },
                    { 3, new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 82, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 54, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 71, 101, 111, 109, 101, 116, 114, 121, 70, 105, 108, 116, 101, 114, 3, 0, 0, 0, 11, 95, 101, 120, 112, 114, 101, 115, 115, 105, 111, 110, 11, 95, 99, 108, 97, 117, 115, 101, 73, 110, 102, 111, 8, 95, 98, 111, 111, 108, 101, 97, 110, 4, 4, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 70, 105, 108, 116, 101, 114, 2, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 2, 0, 0, 0, 1, 2, 0, 0, 0, 10, 9, 3, 0, 0, 0, 0, 5, 3, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 4, 0, 0, 0, 23, 60, 83, 111, 117, 114, 99, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 21, 60, 78, 97, 109, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 25, 60, 79, 112, 101, 114, 97, 116, 111, 114, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 22, 60, 86, 97, 108, 117, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 4, 1, 4, 2, 56, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 73, 110, 102, 111, 83, 111, 117, 114, 99, 101, 2, 0, 0, 0, 64, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 67, 111, 109, 112, 97, 114, 105, 115, 111, 110, 79, 112, 101, 114, 97, 116, 111, 114, 2, 0, 0, 0, 2, 0, 0, 0, 5, 252, 255, 255, 255, 56, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 73, 110, 102, 111, 83, 111, 117, 114, 99, 101, 1, 0, 0, 0, 7, 118, 97, 108, 117, 101, 95, 95, 0, 8, 2, 0, 0, 0, 0, 0, 0, 0, 6, 5, 0, 0, 0, 4, 116, 121, 112, 101, 5, 250, 255, 255, 255, 64, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 67, 111, 109, 112, 97, 114, 105, 115, 111, 110, 79, 112, 101, 114, 97, 116, 111, 114, 1, 0, 0, 0, 7, 118, 97, 108, 117, 101, 95, 95, 0, 8, 2, 0, 0, 0, 6, 0, 0, 0, 6, 7, 0, 0, 0, 8, 104, 111, 115, 112, 105, 116, 97, 108, 11 }, "hospital" },
                    { 4, new byte[] { 0, 1, 0, 0, 0, 255, 255, 255, 255, 1, 0, 0, 0, 0, 0, 0, 0, 12, 2, 0, 0, 0, 82, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 44, 32, 86, 101, 114, 115, 105, 111, 110, 61, 49, 46, 48, 46, 48, 46, 48, 44, 32, 67, 117, 108, 116, 117, 114, 101, 61, 110, 101, 117, 116, 114, 97, 108, 44, 32, 80, 117, 98, 108, 105, 99, 75, 101, 121, 84, 111, 107, 101, 110, 61, 110, 117, 108, 108, 5, 1, 0, 0, 0, 54, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 71, 101, 111, 109, 101, 116, 114, 121, 70, 105, 108, 116, 101, 114, 3, 0, 0, 0, 11, 95, 101, 120, 112, 114, 101, 115, 115, 105, 111, 110, 11, 95, 99, 108, 97, 117, 115, 101, 73, 110, 102, 111, 8, 95, 98, 111, 111, 108, 101, 97, 110, 4, 4, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 70, 105, 108, 116, 101, 114, 2, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 2, 0, 0, 0, 1, 2, 0, 0, 0, 10, 9, 3, 0, 0, 0, 0, 5, 3, 0, 0, 0, 46, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 67, 108, 97, 117, 115, 101, 4, 0, 0, 0, 23, 60, 83, 111, 117, 114, 99, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 21, 60, 78, 97, 109, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 25, 60, 79, 112, 101, 114, 97, 116, 111, 114, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 22, 60, 86, 97, 108, 117, 101, 62, 107, 95, 95, 66, 97, 99, 107, 105, 110, 103, 70, 105, 101, 108, 100, 4, 1, 4, 2, 56, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 73, 110, 102, 111, 83, 111, 117, 114, 99, 101, 2, 0, 0, 0, 64, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 67, 111, 109, 112, 97, 114, 105, 115, 111, 110, 79, 112, 101, 114, 97, 116, 111, 114, 2, 0, 0, 0, 2, 0, 0, 0, 5, 252, 255, 255, 255, 56, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 73, 110, 102, 111, 83, 111, 117, 114, 99, 101, 1, 0, 0, 0, 7, 118, 97, 108, 117, 101, 95, 95, 0, 8, 2, 0, 0, 0, 0, 0, 0, 0, 6, 5, 0, 0, 0, 4, 116, 121, 112, 101, 5, 250, 255, 255, 255, 64, 79, 112, 101, 110, 76, 97, 116, 105, 110, 111, 46, 77, 97, 112, 83, 101, 114, 118, 101, 114, 46, 68, 111, 109, 97, 105, 110, 46, 77, 97, 112, 46, 70, 105, 108, 116, 101, 114, 115, 46, 69, 110, 117, 109, 115, 46, 67, 111, 109, 112, 97, 114, 105, 115, 111, 110, 79, 112, 101, 114, 97, 116, 111, 114, 1, 0, 0, 0, 7, 118, 97, 108, 117, 101, 95, 95, 0, 8, 2, 0, 0, 0, 6, 0, 0, 0, 6, 7, 0, 0, 0, 10, 117, 110, 105, 118, 101, 114, 115, 105, 116, 121, 11 }, "university" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "ID", "Default", "LanguageName" },
                values: new object[,]
                {
                    { 3082, true, "Espa�ol" },
                    { 1033, false, "English" },
                    { 1031, false, "Alem�n" }
                });

            migrationBuilder.InsertData(
                table: "Providers",
                columns: new[] { "Id", "BoundingBoxField", "ConnectionString", "GeoField", "PkField", "Table", "Type" },
                values: new object[,]
                {
                    { 2, "BBOX_latino1", "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\GeoCuba.mdf;Integrated Security=True", "ogr_geometry", "osm_id", "roads", "OpenLatino.MapServer.Infrastucture.SQL.DataSource.ProviderSQL, OpenLatino.MapServer.Infrastructure.SQL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" },
                    { 1, "BBOX_latino", "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\GeoCuba.mdf;Integrated Security=True", "ogr_geometry", "osm_id", "points", "OpenLatino.MapServer.Infrastucture.SQL.DataSource.ProviderSQL, OpenLatino.MapServer.Infrastructure.SQL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" },
                    { 3, "BBOX_latino1", "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\GeoCuba.mdf;Integrated Security=True", "ogr_geometry", "osm_id", "buildings", "OpenLatino.MapServer.Infrastucture.SQL.DataSource.ProviderSQL, OpenLatino.MapServer.Infrastructure.SQL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "WMS" },
                    { 2, "SOP" }
                });

            migrationBuilder.InsertData(
                table: "Styles",
                columns: new[] { "Id", "EnableOutline", "Fill", "ImageContent", "ImageRotation", "ImageScale", "Line", "Name", "OutlinePen", "PointFill", "PointSize" },
                values: new object[,]
                {
                    { 3, false, "Crimson", null, 0f, 0f, "Black", "roads_style", "Black", "Brown", 0f },
                    { 2, false, "Crimson", new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 64, 0, 0, 0, 64, 8, 6, 0, 0, 0, 170, 105, 113, 222, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0, 0, 177, 143, 11, 252, 97, 5, 0, 0, 4, 203, 73, 68, 65, 84, 120, 94, 237, 155, 89, 168, 117, 99, 24, 199, 143, 121, 206, 135, 72, 185, 146, 76, 87, 124, 74, 134, 50, 196, 13, 69, 148, 136, 16, 185, 244, 69, 41, 113, 161, 36, 153, 135, 11, 69, 92, 152, 135, 12, 9, 37, 67, 82, 40, 119, 46, 228, 70, 68, 72, 50, 20, 81, 198, 204, 191, 95, 173, 127, 61, 173, 246, 249, 28, 231, 59, 235, 221, 251, 172, 214, 191, 126, 173, 189, 215, 122, 247, 122, 159, 231, 121, 159, 119, 216, 235, 221, 123, 105, 64, 109, 221, 17, 109, 128, 227, 225, 106, 120, 0, 94, 132, 55, 58, 124, 125, 63, 92, 5, 199, 193, 238, 16, 245, 239, 179, 46, 180, 77, 119, 84, 71, 193, 61, 240, 49, 252, 179, 66, 62, 130, 187, 225, 72, 136, 234, 61, 23, 86, 91, 117, 168, 141, 240, 2, 244, 157, 251, 11, 254, 236, 240, 117, 255, 125, 45, 251, 55, 60, 7, 135, 129, 170, 247, 95, 56, 213, 52, 189, 6, 126, 133, 56, 210, 119, 78, 199, 42, 57, 31, 18, 148, 188, 255, 5, 236, 58, 209, 194, 117, 137, 24, 180, 43, 60, 15, 49, 252, 143, 242, 90, 71, 117, 170, 158, 171, 215, 250, 231, 114, 190, 150, 127, 22, 118, 1, 181, 48, 65, 136, 33, 123, 194, 219, 160, 161, 26, 93, 157, 170, 173, 25, 126, 134, 239, 224, 247, 114, 110, 57, 106, 32, 222, 130, 61, 64, 205, 61, 8, 246, 71, 141, 216, 25, 28, 205, 53, 112, 86, 171, 231, 253, 155, 112, 37, 156, 12, 251, 131, 173, 121, 40, 124, 1, 41, 159, 178, 179, 72, 176, 94, 135, 29, 193, 186, 231, 58, 38, 100, 100, 126, 24, 170, 129, 82, 157, 121, 25, 156, 2, 103, 105, 31, 248, 12, 44, 215, 31, 4, 103, 145, 58, 156, 54, 213, 220, 102, 135, 84, 124, 49, 84, 195, 36, 142, 120, 238, 50, 136, 182, 133, 211, 224, 46, 48, 149, 223, 135, 239, 33, 159, 91, 41, 201, 178, 11, 64, 53, 15, 66, 82, 111, 95, 248, 26, 116, 56, 78, 219, 242, 242, 19, 156, 10, 145, 142, 191, 3, 213, 145, 240, 95, 169, 223, 199, 186, 252, 204, 151, 96, 6, 53, 159, 30, 19, 241, 91, 64, 131, 210, 207, 53, 42, 35, 253, 233, 16, 93, 7, 213, 120, 175, 91, 46, 142, 228, 218, 255, 33, 117, 222, 0, 202, 236, 106, 162, 68, 122, 47, 248, 22, 116, 32, 173, 31, 163, 174, 133, 232, 118, 200, 181, 92, 95, 11, 18, 188, 111, 192, 25, 72, 53, 201, 130, 68, 58, 125, 63, 206, 199, 160, 247, 96, 123, 80, 41, 99, 139, 175, 182, 165, 55, 71, 2, 154, 177, 160, 73, 22, 100, 238, 117, 81, 162, 83, 49, 34, 129, 184, 8, 212, 126, 96, 134, 120, 62, 215, 214, 154, 4, 246, 41, 80, 131, 175, 11, 146, 98, 206, 193, 117, 234, 74, 235, 154, 142, 118, 13, 101, 55, 136, 145, 49, 120, 173, 73, 189, 159, 64, 178, 110, 80, 37, 194, 135, 192, 111, 16, 35, 226, 164, 203, 96, 101, 42, 190, 11, 53, 67, 134, 32, 1, 240, 123, 199, 129, 160, 6, 205, 130, 140, 254, 174, 228, 170, 1, 9, 192, 77, 160, 14, 130, 31, 161, 150, 25, 138, 220, 255, 68, 80, 131, 174, 9, 114, 243, 51, 193, 74, 211, 186, 57, 94, 10, 150, 57, 182, 123, 63, 180, 243, 146, 186, 51, 237, 14, 58, 16, 230, 230, 231, 128, 149, 166, 229, 99, 196, 38, 80, 167, 64, 61, 63, 36, 177, 225, 44, 80, 77, 2, 208, 207, 128, 180, 244, 167, 224, 224, 103, 255, 247, 253, 80, 163, 127, 165, 159, 1, 77, 186, 192, 73, 96, 165, 58, 222, 34, 205, 151, 163, 214, 127, 2, 168, 65, 3, 144, 17, 246, 96, 200, 19, 159, 26, 0, 95, 155, 146, 45, 90, 94, 82, 183, 79, 140, 14, 0, 53, 232, 44, 144, 117, 192, 14, 96, 186, 91, 121, 43, 103, 103, 145, 186, 125, 224, 154, 117, 192, 224, 203, 225, 68, 248, 105, 72, 139, 247, 13, 107, 133, 117, 107, 195, 227, 160, 6, 77, 255, 40, 3, 225, 133, 160, 17, 139, 144, 1, 231, 130, 106, 18, 128, 164, 152, 207, 229, 92, 250, 218, 2, 233, 139, 45, 209, 121, 235, 253, 10, 178, 145, 210, 228, 219, 160, 74, 164, 175, 7, 141, 105, 49, 223, 247, 73, 215, 203, 87, 239, 38, 173, 31, 25, 105, 217, 27, 124, 160, 105, 75, 180, 236, 10, 105, 253, 207, 193, 47, 95, 142, 75, 205, 90, 63, 74, 196, 207, 3, 141, 106, 53, 24, 234, 120, 234, 58, 27, 84, 211, 214, 175, 74, 197, 247, 129, 6, 213, 7, 163, 67, 145, 58, 220, 111, 84, 115, 115, 94, 153, 118, 166, 159, 115, 240, 171, 80, 13, 28, 130, 220, 251, 37, 216, 14, 230, 146, 250, 125, 197, 128, 221, 224, 53, 208, 192, 204, 207, 213, 248, 45, 161, 166, 253, 43, 144, 237, 177, 185, 59, 31, 101, 113, 180, 19, 184, 40, 137, 225, 91, 58, 59, 84, 199, 229, 17, 240, 105, 148, 90, 24, 231, 163, 106, 208, 229, 240, 3, 196, 112, 157, 200, 200, 157, 115, 203, 97, 25, 203, 86, 199, 221, 60, 201, 87, 109, 181, 112, 206, 71, 26, 150, 108, 240, 177, 217, 147, 208, 207, 2, 223, 7, 157, 148, 122, 174, 95, 246, 9, 240, 203, 151, 90, 136, 62, 191, 18, 213, 145, 249, 8, 184, 19, 62, 132, 234, 220, 230, 248, 0, 238, 0, 127, 100, 17, 205, 117, 180, 95, 141, 108, 173, 100, 131, 114, 166, 240, 39, 47, 151, 192, 109, 240, 24, 248, 16, 213, 95, 128, 248, 250, 86, 240, 154, 101, 234, 19, 222, 254, 125, 214, 157, 52, 126, 53, 173, 231, 103, 214, 181, 227, 85, 58, 98, 223, 245, 185, 157, 107, 247, 51, 224, 24, 56, 188, 227, 104, 112, 243, 212, 159, 214, 184, 155, 84, 199, 146, 81, 40, 25, 224, 182, 248, 172, 62, 95, 121, 6, 212, 160, 15, 55, 91, 43, 1, 184, 17, 28, 217, 93, 209, 57, 221, 101, 90, 244, 232, 57, 175, 229, 135, 15, 163, 12, 192, 205, 160, 195, 253, 233, 174, 158, 123, 8, 212, 20, 128, 238, 56, 10, 77, 1, 232, 142, 83, 0, 160, 58, 91, 153, 2, 208, 29, 167, 0, 128, 154, 2, 208, 29, 71, 161, 41, 0, 221, 113, 10, 0, 84, 103, 43, 83, 0, 186, 227, 20, 0, 80, 83, 0, 186, 227, 40, 52, 5, 160, 59, 78, 1, 128, 234, 108, 101, 10, 64, 119, 156, 2, 0, 106, 148, 1, 240, 183, 196, 213, 217, 74, 206, 61, 8, 106, 148, 1, 112, 67, 196, 125, 191, 229, 2, 224, 181, 71, 65, 141, 50, 0, 231, 131, 206, 206, 218, 36, 213, 121, 143, 87, 128, 202, 103, 70, 35, 55, 59, 108, 213, 123, 193, 255, 26, 196, 97, 49, 32, 110, 146, 186, 17, 234, 159, 48, 45, 219, 96, 19, 116, 105, 233, 95, 57, 210, 207, 187, 159, 84, 146, 147, 0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130 }, 0f, 0f, "Red", "points_style", "Black", "Brown", 0f },
                    { 4, true, "LawnGreen", null, 0f, 0f, "DarkOliveGreen", "school_style", "Black", "Brown", 0f },
                    { 5, true, "DarkMagenta", null, 0f, 0f, "CornflowerBlue", "hospital_style", "Black", "Brown", 0f },
                    { 6, true, "DarkRed", null, 0f, 0f, "DarkBlue", "university_style", "Black", "Brown", 0f },
                    { 7, true, "Tomato", null, 0f, 0f, "Red", "buildingThematic_style", "Black", "Brown", 0f },
                    { 1, true, "LightYellow", null, 0f, 0f, "Red", "buildings_style", "Black", "Brown", 0f }
                });

            migrationBuilder.InsertData(
                table: "WorkSpaces",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "common" });

            migrationBuilder.InsertData(
                table: "ClientWorkSpaces",
                columns: new[] { "Client_Id", "WorkSpace_Id" },
                values: new object[] { "1", 1 });

            migrationBuilder.InsertData(
                table: "Layers",
                columns: new[] { "Id", "Order", "ProviderInfoId" },
                values: new object[,]
                {
                    { 2, 0, 1 },
                    { 3, 2, 2 },
                    { 1, 3, 3 },
                    { 4, 4, 3 }
                });

            migrationBuilder.InsertData(
                table: "ProviderTranslations",
                columns: new[] { "LanguageID", "EntityID", "Description", "Name" },
                values: new object[,]
                {
                    { 3082, 1, "Descripci�n de latino", "Latino DB" },
                    { 3082, 2, "Descripci�n de latino1", "Latino1 DB" },
                    { 3082, 3, "Descripci�n de latino2", "Latino2 DB" },
                    { 1033, 1, "Latino's description", "Latino name" }
                });

            migrationBuilder.InsertData(
                table: "ServiceFunctions",
                columns: new[] { "Id", "Name", "ServiceId" },
                values: new object[,]
                {
                    { 1, "GetMap", 1 },
                    { 2, "GetFeatureInfo", 1 },
                    { 3, "GetCapabilities", 1 },
                    { 4, "GetGraphicLegend", 1 }
                });

            migrationBuilder.InsertData(
                table: "AlfaInfos",
                columns: new[] { "ID", "Columns", "ConnectionString", "LayerID", "PkField", "Table" },
                values: new object[,]
                {
                    { 3, "timestamp,name,type", "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True", 2, "osm_id", "points" },
                    { 1, "type,oneway,bridge,maxspeed,name,ref", "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True", 3, "osm_id", "roads" },
                    { 2, "name,type", "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True", 1, "osm_id", "buildings" },
                    { 4, "name,type", "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True", 4, "osm_id", "buildings" }
                });

            migrationBuilder.InsertData(
                table: "LayerStyles",
                columns: new[] { "LayerId", "VectorStyleId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 3, 3 },
                    { 1, 1 },
                    { 4, 7 }
                });

            migrationBuilder.InsertData(
                table: "LayerTranslations",
                columns: new[] { "LanguageID", "EntityID", "Description", "Name" },
                values: new object[,]
                {
                    { 1033, 2, "This lay contains all buildings", "Buildings" },
                    { 3082, 2, "En esta capa est�n representados todos los edificios, casas e industrias.", "Edificios" },
                    { 1033, 3, "All hotels", "Hotel layer" },
                    { 3082, 1, "Esta capa representa a todas las capas de la ciudad", "calles" },
                    { 3082, 4, "Esta capa representa a todas las capas de la ciudad", "Building Thematic" }
                });

            migrationBuilder.InsertData(
                table: "StyleConfiguration",
                columns: new[] { "LayerId", "FilterId", "StyleId" },
                values: new object[,]
                {
                    { 4, 2, 4 },
                    { 4, 3, 5 },
                    { 4, 4, 6 }
                });

            migrationBuilder.InsertData(
                table: "AlfaInfoTranslations",
                columns: new[] { "LanguageID", "EntityID", "Description", "Name" },
                values: new object[,]
                {
                    { 1033, 3, "timestamp, name and type of points", "points info" },
                    { 1033, 1, "contain roads names, max speed and other infos", "Roads Info" },
                    { 3082, 2, "La informaci�n que contiene de las edificaciones es: tipo de edificaci�n (oficina, hotel, ...) y nombre", "Informaci�n de las construcciones" },
                    { 3082, 4, "La informaci�n que contiene de las edificaciones es: tipo de edificaci�n (oficina, hotel, ...) y nombre", "Informaci�n de las construcciones" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LayerID",
                table: "AlfaInfos",
                column: "LayerID");

            migrationBuilder.CreateIndex(
                name: "IX_EntityID",
                table: "AlfaInfoTranslations",
                column: "EntityID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageID",
                table: "AlfaInfoTranslations",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Client_Id",
                table: "ClientWorkSpaces",
                column: "Client_Id");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpace_Id",
                table: "ClientWorkSpaces",
                column: "WorkSpace_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderInfoId",
                table: "Layers",
                column: "ProviderInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_LayerStyles_VectorStyleId",
                table: "LayerStyles",
                column: "VectorStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityID",
                table: "LayerTranslations",
                column: "EntityID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageID",
                table: "LayerTranslations",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_Layer_Id",
                table: "LayerWorkspaces",
                column: "Layer_Id");

            migrationBuilder.CreateIndex(
                name: "IX_LayerWorkspaces_VectorStyleId",
                table: "LayerWorkspaces",
                column: "VectorStyleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSpace_Id",
                table: "LayerWorkspaces",
                column: "WorkSpace_Id");

            migrationBuilder.CreateIndex(
                name: "IX_EntityID",
                table: "ProviderTranslations",
                column: "EntityID");

            migrationBuilder.CreateIndex(
                name: "IX_LanguageID",
                table: "ProviderTranslations",
                column: "LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceId",
                table: "ServiceFunctions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_FilterId",
                table: "StyleConfiguration",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_LayerId",
                table: "StyleConfiguration",
                column: "LayerId");

            migrationBuilder.CreateIndex(
                name: "IX_StyleId",
                table: "StyleConfiguration",
                column: "StyleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__MigrationHistory");

            migrationBuilder.DropTable(
                name: "AlfaInfoTranslations");

            migrationBuilder.DropTable(
                name: "ClientWorkSpaces");

            migrationBuilder.DropTable(
                name: "LayerStyles");

            migrationBuilder.DropTable(
                name: "LayerTranslations");

            migrationBuilder.DropTable(
                name: "LayerWorkspaces");

            migrationBuilder.DropTable(
                name: "ProviderTranslations");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "ServiceFunctions");

            migrationBuilder.DropTable(
                name: "StyleConfiguration");

            migrationBuilder.DropTable(
                name: "AlfaInfos");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "WorkSpaces");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "Styles");

            migrationBuilder.DropTable(
                name: "Layers");

            migrationBuilder.DropTable(
                name: "Providers");
        }
    }
}
