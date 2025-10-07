using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class addsenatorbudgetid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SenatorBudgetId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_SenatorBudgetId",
                table: "Loans",
                column: "SenatorBudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_SenatorBudgets_SenatorBudgetId",
                table: "Loans",
                column: "SenatorBudgetId",
                principalTable: "SenatorBudgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_SenatorBudgets_SenatorBudgetId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_SenatorBudgetId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "SenatorBudgetId",
                table: "Loans");
        }
    }
}
