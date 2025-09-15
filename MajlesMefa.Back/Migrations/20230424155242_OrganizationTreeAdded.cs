using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationTreeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "OrganizationEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationEntity_ParentId",
                table: "OrganizationEntity",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntity_OrganizationEntity_ParentId",
                table: "OrganizationEntity",
                column: "ParentId",
                principalTable: "OrganizationEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntity_OrganizationEntity_ParentId",
                table: "OrganizationEntity");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationEntity_ParentId",
                table: "OrganizationEntity");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "OrganizationEntity");
        }
    }
}
