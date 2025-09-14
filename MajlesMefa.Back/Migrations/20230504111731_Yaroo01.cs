using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class Yaroo01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Mokatebes",
                type: "decimal(19,4)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAmount",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValueSql: "0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Mokatebes");

            migrationBuilder.DropColumn(
                name: "HasAmount",
                table: "Categories");
        }
    }
}
