using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class NotghCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AnswerFromProUnitDate",
                table: "Notghs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AnswerFromProUnitDesc",
                table: "Notghs",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnswerFromProUnitNo",
                table: "Notghs",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Chekide",
                table: "Notghs",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GardeshErjaat",
                table: "Notghs",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "JalaseAlaniDate",
                table: "Notghs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerFromProUnitDate",
                table: "Notghs");

            migrationBuilder.DropColumn(
                name: "AnswerFromProUnitDesc",
                table: "Notghs");

            migrationBuilder.DropColumn(
                name: "AnswerFromProUnitNo",
                table: "Notghs");

            migrationBuilder.DropColumn(
                name: "Chekide",
                table: "Notghs");

            migrationBuilder.DropColumn(
                name: "GardeshErjaat",
                table: "Notghs");

            migrationBuilder.DropColumn(
                name: "JalaseAlaniDate",
                table: "Notghs");
        }
    }
}
