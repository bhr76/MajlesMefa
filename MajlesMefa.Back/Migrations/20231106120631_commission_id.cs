using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class commission_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RelatedComission",
                table: "Tarhs",
                type: "uniqueidentifier",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<Guid>(
                name: "Commission",
                table: "TahghighTafahoses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<Guid>(
                name: "Commission",
                table: "Sovals",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<Guid>(
                name: "ComissionMembership",
                table: "SenatorProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DastoorJalasatComissionEntityDataEntryId",
                table: "SenatorProfiles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Commission",
                table: "Layehes",
                type: "uniqueidentifier",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.CreateIndex(
                name: "IX_SenatorProfiles_DastoorJalasatComissionEntityDataEntryId",
                table: "SenatorProfiles",
                column: "DastoorJalasatComissionEntityDataEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_SenatorProfiles_DastoorJalasatComissions_DastoorJalasatComissionEntityDataEntryId",
                table: "SenatorProfiles",
                column: "DastoorJalasatComissionEntityDataEntryId",
                principalTable: "DastoorJalasatComissions",
                principalColumn: "DataEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SenatorProfiles_DastoorJalasatComissions_DastoorJalasatComissionEntityDataEntryId",
                table: "SenatorProfiles");

            migrationBuilder.DropIndex(
                name: "IX_SenatorProfiles_DastoorJalasatComissionEntityDataEntryId",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "DastoorJalasatComissionEntityDataEntryId",
                table: "SenatorProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "RelatedComission",
                table: "Tarhs",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "Commission",
                table: "TahghighTafahoses",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Commission",
                table: "Sovals",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ComissionMembership",
                table: "SenatorProfiles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Commission",
                table: "Layehes",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 64);
        }
    }
}
