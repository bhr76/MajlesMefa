using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class bUserRoleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_BussinessRoles_RoleId",
                table: "PageRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_Pages_PageId",
                table: "PageRoles");

            migrationBuilder.DropColumn(
                name: "PgaeId",
                table: "PageRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "PageId",
                table: "PageRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_BussinessRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "BussinessRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_BussinessRoles_PageId",
                table: "PageRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_PageRoles_Pages_RoleId",
                table: "PageRoles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.AlterColumn<Guid>(
                name: "PageId",
                table: "PageRoles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PgaeId",
                table: "PageRoles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                principalColumn: "Id");
        }
    }
}
