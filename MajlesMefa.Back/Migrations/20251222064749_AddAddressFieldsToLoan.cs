using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressFieldsToLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Loans",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CityId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProvinceId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loan_CityId",
                table: "Loans",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_ProvinceId",
                table: "Loans",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Cities_CityId",
                table: "Loans",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Cities_ProvinceId",
                table: "Loans",
                column: "ProvinceId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Cities_CityId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Cities_ProvinceId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loan_CityId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loan_ProvinceId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Loans");
        }
    }
}
