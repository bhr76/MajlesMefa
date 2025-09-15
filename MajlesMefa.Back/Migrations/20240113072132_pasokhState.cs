using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class pasokhState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "PasokhState",
                table: "TazakorShafahis",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "PasokhState",
                table: "TazakorKatbis",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "PasokhState",
                table: "Notghs",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasokhState",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "PasokhState",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "PasokhState",
                table: "Notghs");
        }
    }
}
