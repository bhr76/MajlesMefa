using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class update_molaghat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mosavabat",
                table: "Molaghats");

            migrationBuilder.DropColumn(
                name: "SavabeghMolaghat",
                table: "Molaghats");

            migrationBuilder.DropColumn(
                name: "SavabeghSovalat",
                table: "Molaghats");

            migrationBuilder.DropColumn(
                name: "SavabeghTazakorat",
                table: "Molaghats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mosavabat",
                table: "Molaghats",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SavabeghMolaghat",
                table: "Molaghats",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SavabeghSovalat",
                table: "Molaghats",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SavabeghTazakorat",
                table: "Molaghats",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: true);
        }
    }
}
