using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(124)", maxLength: 124, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DataEntryType = table.Column<short>(type: "smallint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    DataEntryType = table.Column<short>(type: "smallint", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DataEntries_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataEntries_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DataEntries_Users_SenatorId",
                        column: x => x.SenatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActionReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActRefType = table.Column<short>(type: "smallint", nullable: false),
                    IsVisibleForSenator = table.Column<bool>(type: "bit", nullable: false),
                    SeenDateBySenator = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SeenDateByToUser = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionReferences_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionReferences_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionReferences_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mokatebes",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MokatebeKonande = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MokatebeType = table.Column<byte>(type: "tinyint", nullable: false),
                    ShomareDabirkhane = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ShomareDabirkhaneMarkazi = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    TarikhDabirKhane = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TarikhDabirKhaneMarkazi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GardeshErjaat = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mokatebes", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Mokatebes_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notghs",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notghs", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Notghs_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Peygiries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeygiriKonande = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    PeygiriDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peygiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peygiries_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TazakorKatbis",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TazakorKatbis", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_TazakorKatbis_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TazakorShafahis",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TazakorShafahis", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_TazakorShafahis_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionReferences_Created",
                table: "ActionReferences",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_ActionReferences_DataEntryId",
                table: "ActionReferences",
                column: "DataEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionReferences_FromUserId",
                table: "ActionReferences",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionReferences_ToUserId",
                table: "ActionReferences",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Created",
                table: "Categories",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Order",
                table: "Categories",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_CategoryId",
                table: "DataEntries",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_Created",
                table: "DataEntries",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_CreatorId",
                table: "DataEntries",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_SenatorId",
                table: "DataEntries",
                column: "SenatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Peygiries_Created",
                table: "Peygiries",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Peygiries_DataEntryId",
                table: "Peygiries",
                column: "DataEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Created",
                table: "Users",
                column: "Created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActionReferences");

            migrationBuilder.DropTable(
                name: "Mokatebes");

            migrationBuilder.DropTable(
                name: "Notghs");

            migrationBuilder.DropTable(
                name: "Peygiries");

            migrationBuilder.DropTable(
                name: "TazakorKatbis");

            migrationBuilder.DropTable(
                name: "TazakorShafahis");

            migrationBuilder.DropTable(
                name: "DataEntries");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
