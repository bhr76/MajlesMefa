using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class SootiFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_BussinessRoles_PageId",
                table: "PageRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_Pages_RoleId",
                table: "PageRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_PageRoles_BussinessRoles_RoleId",
                table: "PageRoles",
                column: "RoleId",
                principalTable: "BussinessRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageRoles_Pages_PageId",
                table: "PageRoles",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_BussinessRoles_RoleId",
                table: "PageRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_Pages_PageId",
                table: "PageRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_PageRoles_BussinessRoles_PageId",
                table: "PageRoles",
                column: "PageId",
                principalTable: "BussinessRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PageRoles_Pages_RoleId",
                table: "PageRoles",
                column: "RoleId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
