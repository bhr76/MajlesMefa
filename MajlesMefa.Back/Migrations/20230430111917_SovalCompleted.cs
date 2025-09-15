using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class SovalCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sovals",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Commission = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    KarbargDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DabirkhaneMakaziNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MohlatZamani = table.Column<byte>(type: "tinyint", nullable: false),
                    GardeshErjaat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    JalasatDakheli = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    BarresiDarCommission = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    BarresiDarSahn = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sovals", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Sovals_DataEntries_DataEntryId",
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
                name: "Sovals");
        }
    }
}
