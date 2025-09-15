using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class someChangeInDataEntries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLogicallyDeleted",
                table: "ActionReferences");

            migrationBuilder.CreateTable(
                name: "Khadamat",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TarikhKhedmat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Khedmat = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khadamat", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Khadamat_DataEntries_DataEntryId",
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
                name: "Khadamat");

            migrationBuilder.AddColumn<bool>(
                name: "IsLogicallyDeleted",
                table: "ActionReferences",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
