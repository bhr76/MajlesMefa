using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class referenceTo_changed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionReferences_Organizations_ToUserId",
                table: "ActionReferences");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionReferences_Users_ToUserId",
                table: "ActionReferences",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionReferences_Users_ToUserId",
                table: "ActionReferences");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionReferences_Organizations_ToUserId",
                table: "ActionReferences",
                column: "ToUserId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
