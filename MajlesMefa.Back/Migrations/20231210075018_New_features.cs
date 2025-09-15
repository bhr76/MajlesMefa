using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class New_features : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Mahal",
                table: "Molaghats",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SenatorProfiles_ComissionMembership",
                table: "SenatorProfiles",
                column: "ComissionMembership");

            migrationBuilder.AddForeignKey(
                name: "FK_SenatorProfiles_DastoorJalasatComissions_ComissionMembership",
                table: "SenatorProfiles",
                column: "ComissionMembership",
                principalTable: "DastoorJalasatComissions",
                principalColumn: "DataEntryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SenatorProfiles_DastoorJalasatComissions_ComissionMembership",
                table: "SenatorProfiles");

            migrationBuilder.DropIndex(
                name: "IX_SenatorProfiles_ComissionMembership",
                table: "SenatorProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "Mahal",
                table: "Molaghats",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
