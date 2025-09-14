using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class LayeheAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErsalBeVazir",
                table: "Tarhs",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Layehes",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Commission = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TaghdimBeMajlesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JalasateBarresiDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsConfirmedInNatijeJalasatCommsion = table.Column<bool>(type: "bit", nullable: true),
                    BarresiKollyatDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NazarNamayandeghan = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    ChekideMehvar = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    NahveBarresi = table.Column<int>(type: "int", nullable: false),
                    NatijeBarresiKollyat = table.Column<byte>(type: "tinyint", nullable: false),
                    NatijeBarresiKollyatVotePCount = table.Column<int>(type: "int", nullable: false),
                    NatijeBarresiKollyatVoteNCount = table.Column<int>(type: "int", nullable: false),
                    NatijeBarresiMavad = table.Column<byte>(type: "tinyint", nullable: false),
                    NatijeBarresiMavadVotePCount = table.Column<int>(type: "int", nullable: false),
                    NatijeBarresiMavadVoteNCount = table.Column<int>(type: "int", nullable: false),
                    ErjaBeShoraDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NatijeBarresiShora = table.Column<byte>(type: "tinyint", nullable: false),
                    EblaghBeGovernment = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    EblaghBeVozara = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layehes", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Layehes_DataEntries_DataEntryId",
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
                name: "Layehes");

            migrationBuilder.DropColumn(
                name: "ErsalBeVazir",
                table: "Tarhs");
        }
    }
}
