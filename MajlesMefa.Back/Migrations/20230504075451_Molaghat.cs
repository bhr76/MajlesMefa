using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class Molaghat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Molaghats",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<short>(type: "smallint", nullable: false),
                    SavabeghMolaghat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SavabeghSovalat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    SavabeghTazakorat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Mahal = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mosavabat = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Commision = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Fracsion = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    MolaghatType = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Molaghats", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Molaghats_DataEntries_DataEntryId",
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
                name: "Molaghats");
        }
    }
}
