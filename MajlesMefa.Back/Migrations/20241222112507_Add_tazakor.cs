using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class Add_tazakor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tazakors",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GheraatSahnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PasokhNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PasokhState = table.Column<byte>(type: "tinyint", nullable: false),
                    PasokhDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShomareName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PishnevisDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TazakorType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tazakors", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Tazakors_DataEntries_DataEntryId",
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
                name: "Tazakors");
        }
    }
}
