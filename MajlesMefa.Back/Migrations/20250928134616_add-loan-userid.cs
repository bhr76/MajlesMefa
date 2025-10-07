using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class addloanuserid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Banks_SuggestedBankId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "SuggestedBankId",
                table: "Loans",
                newName: "BankEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_SuggestedBankId",
                table: "Loans",
                newName: "IX_Loans_BankEntityId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Loans_UserId",
                table: "Loans",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Banks_BankEntityId",
                table: "Loans",
                column: "BankEntityId",
                principalTable: "Banks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Banks_BankEntityId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_UserId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_UserId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "BankEntityId",
                table: "Loans",
                newName: "SuggestedBankId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_BankEntityId",
                table: "Loans",
                newName: "IX_Loans_SuggestedBankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Banks_SuggestedBankId",
                table: "Loans",
                column: "SuggestedBankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
