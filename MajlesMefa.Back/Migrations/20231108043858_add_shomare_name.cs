using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_shomare_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasokhNo",
                table: "TazakorShafahis",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasokhNo",
                table: "TazakorKatbis",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasokhNo",
                table: "Notghs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasokhNo",
                table: "Molaghats",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasokhNo",
                table: "Mokatebes",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasokhNo",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "PasokhNo",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "PasokhNo",
                table: "Notghs");

            migrationBuilder.DropColumn(
                name: "PasokhNo",
                table: "Molaghats");

            migrationBuilder.DropColumn(
                name: "PasokhNo",
                table: "Mokatebes");
        }
    }
}
