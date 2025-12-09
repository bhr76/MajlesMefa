using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class modifiedDataonLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedDate",
                table: "Loans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedUserId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedDate",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LastModifiedUserId",
                table: "Loans");
        }
    }
}
