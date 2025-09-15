using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_organization_to_peygiri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PeygiriKonandeId",
                table: "Peygiries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Peygiries_PeygiriKonandeId",
                table: "Peygiries",
                column: "PeygiriKonandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Peygiries_Organizations_PeygiriKonandeId",
                table: "Peygiries",
                column: "PeygiriKonandeId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Peygiries_Organizations_PeygiriKonandeId",
                table: "Peygiries");

            migrationBuilder.DropIndex(
                name: "IX_Peygiries_PeygiriKonandeId",
                table: "Peygiries");

            migrationBuilder.DropColumn(
                name: "PeygiriKonandeId",
                table: "Peygiries");
        }
    }
}
