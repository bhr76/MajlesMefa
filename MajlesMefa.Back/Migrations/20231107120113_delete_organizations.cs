using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class delete_organizations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organization",
                table: "TahghighTafahoses");

            migrationBuilder.DropColumn(
                name: "Organization",
                table: "Sovals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "TahghighTafahoses",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "Sovals",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
