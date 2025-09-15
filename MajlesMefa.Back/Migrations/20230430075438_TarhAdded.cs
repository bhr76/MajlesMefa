using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class TarhAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mokatebes_DataEntries_DataEntryId",
                table: "Mokatebes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notghs_DataEntries_DataEntryId",
                table: "Notghs");

            migrationBuilder.DropForeignKey(
                name: "FK_TazakorKatbis_DataEntries_DataEntryId",
                table: "TazakorKatbis");

            migrationBuilder.DropForeignKey(
                name: "FK_TazakorShafahis_DataEntries_DataEntryId",
                table: "TazakorShafahis");

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RepeatCount = table.Column<int>(type: "int", nullable: false),
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Keywords_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarhs",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Shenase = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    NamayandeghanEmzaKonande = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    RelatedComission = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Gardeshkar = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    HamahangiBaraSherkat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    NamayandeghanBaraSherkat = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    GhozareshMozakerat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    NatijeBarresi = table.Column<byte>(type: "tinyint", nullable: false),
                    VazeyatBarresi = table.Column<byte>(type: "tinyint", nullable: false),
                    NazarNamayande = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Kollyat = table.Column<byte>(type: "tinyint", nullable: false),
                    MavadTarh = table.Column<byte>(type: "tinyint", nullable: false),
                    NatijeBarresiShoraNegahban = table.Column<byte>(type: "tinyint", nullable: false),
                    NatijeBarresiShoraNegahbanDescription = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SavabeghEblagh = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    GhozarshNahayi = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Havashi = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarhs", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Tarhs_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Created",
                table: "Keywords",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_DataEntryId",
                table: "Keywords",
                column: "DataEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_Name_DataEntryId",
                table: "Keywords",
                columns: new[] { "Name", "DataEntryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mokatebes_DataEntries_DataEntryId",
                table: "Mokatebes",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notghs_DataEntries_DataEntryId",
                table: "Notghs",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TazakorKatbis_DataEntries_DataEntryId",
                table: "TazakorKatbis",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TazakorShafahis_DataEntries_DataEntryId",
                table: "TazakorShafahis",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mokatebes_DataEntries_DataEntryId",
                table: "Mokatebes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notghs_DataEntries_DataEntryId",
                table: "Notghs");

            migrationBuilder.DropForeignKey(
                name: "FK_TazakorKatbis_DataEntries_DataEntryId",
                table: "TazakorKatbis");

            migrationBuilder.DropForeignKey(
                name: "FK_TazakorShafahis_DataEntries_DataEntryId",
                table: "TazakorShafahis");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Tarhs");

            migrationBuilder.AddForeignKey(
                name: "FK_Mokatebes_DataEntries_DataEntryId",
                table: "Mokatebes",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notghs_DataEntries_DataEntryId",
                table: "Notghs",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TazakorKatbis_DataEntries_DataEntryId",
                table: "TazakorKatbis",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TazakorShafahis_DataEntries_DataEntryId",
                table: "TazakorShafahis",
                column: "DataEntryId",
                principalTable: "DataEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
