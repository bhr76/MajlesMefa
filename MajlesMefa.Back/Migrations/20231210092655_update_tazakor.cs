using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class update_tazakor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GardeshErjaat",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "GardeshErjaatMoavenat",
                table: "TazakorShafahis");

            migrationBuilder.DropColumn(
                name: "GardeshErjaat",
                table: "TazakorKatbis");

            migrationBuilder.DropColumn(
                name: "GardeshErjaatMoavenat",
                table: "TazakorKatbis");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
