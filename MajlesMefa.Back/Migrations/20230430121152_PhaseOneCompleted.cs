using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class PhaseOneCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GardeshErjaat",
                table: "TazakorShafahis",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GardeshErjaatMoavenat",
                table: "TazakorShafahis",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "GheraatSahnDate",
                table: "TazakorShafahis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PishnevisDate",
                table: "TazakorShafahis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShomareName",
                table: "TazakorShafahis",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GardeshErjaat",
                table: "TazakorKatbis",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GardeshErjaatMoavenat",
                table: "TazakorKatbis",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "GheraatSahnDate",
                table: "TazakorKatbis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PishnevisDate",
                table: "TazakorKatbis",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShomareName",
                table: "TazakorKatbis",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DastoorJalasatComissions",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ghozareshat = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    DateAndDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DastoorJalasatComissions", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_DastoorJalasatComissions_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EzhaaratResaneeees",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Manba = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EzhaaratResaneeees", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_EzhaaratResaneeees_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TahghighTafahoses",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShomareName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    ShomareDaryaft = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Commission = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Mokhatab = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TahghighTafahosVazyat = table.Column<byte>(type: "tinyint", nullable: false),
                    AzaKomite = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TahghighTafahoses", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_TahghighTafahoses_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DastoorJalasatComissions");

            migrationBuilder.DropTable(
                name: "EzhaaratResaneeees");

            migrationBuilder.DropTable(
                name: "TahghighTafahoses");

            migrationBuilder.DropColumn(
                name: "GardeshErjaat",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "GardeshErjaatMoavenat",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "GheraatSahnDate",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "PishnevisDate",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "ShomareName",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "GardeshErjaat",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "GardeshErjaatMoavenat",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "GheraatSahnDate",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "PishnevisDate",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "ShomareName",
                table: "TazakorKatbis");
        }
    }
}
