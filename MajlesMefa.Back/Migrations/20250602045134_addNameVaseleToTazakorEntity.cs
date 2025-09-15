using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class addNameVaseleToTazakorEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameVaseleDabirkhaneNo",
                table: "Tazakors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NameVaseleDate",
                table: "Tazakors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NameVaseleNo",
                table: "Tazakors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "VaseleAz",
                table: "Tazakors",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameVaseleDabirkhaneNo",
                table: "Tazakors");

            migrationBuilder.DropColumn(
                name: "NameVaseleDate",
                table: "Tazakors");

            migrationBuilder.DropColumn(
                name: "NameVaseleNo",
                table: "Tazakors");

            migrationBuilder.DropColumn(
                name: "VaseleAz",
                table: "Tazakors");
        }
    }
}
