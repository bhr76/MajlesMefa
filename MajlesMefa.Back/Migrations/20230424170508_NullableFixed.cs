using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class NullableFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CityEntity_CityEntity_ParentCityId",
                table: "CityEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationEntity_OrganizationEntity_ParentId",
                table: "OrganizationEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CityEntity_CityId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_OrganizationEntity_OrganizationId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationEntity",
                table: "OrganizationEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CityEntity",
                table: "CityEntity");

            migrationBuilder.RenameTable(
                name: "OrganizationEntity",
                newName: "Organizations");

            migrationBuilder.RenameTable(
                name: "CityEntity",
                newName: "Cities");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationEntity_ParentId",
                table: "Organizations",
                newName: "IX_Organizations_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_OrganizationEntity_Created",
                table: "Organizations",
                newName: "IX_Organizations_Created");

            migrationBuilder.RenameIndex(
                name: "IX_CityEntity_ParentCityId",
                table: "Cities",
                newName: "IX_Cities_ParentCityId");

            migrationBuilder.RenameIndex(
                name: "IX_CityEntity_Created",
                table: "Cities",
                newName: "IX_Cities_Created");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentCityId",
                table: "Cities",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cities",
                table: "Cities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Cities_ParentCityId",
                table: "Cities",
                column: "ParentCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_Organizations_ParentId",
                table: "Organizations",
                column: "ParentId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Cities_CityId",
                table: "Users",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                table: "Users",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Cities_ParentCityId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_Organizations_ParentId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Cities_CityId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cities",
                table: "Cities");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "OrganizationEntity");

            migrationBuilder.RenameTable(
                name: "Cities",
                newName: "CityEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Organizations_ParentId",
                table: "OrganizationEntity",
                newName: "IX_OrganizationEntity_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Organizations_Created",
                table: "OrganizationEntity",
                newName: "IX_OrganizationEntity_Created");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ParentCityId",
                table: "CityEntity",
                newName: "IX_CityEntity_ParentCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_Created",
                table: "CityEntity",
                newName: "IX_CityEntity_Created");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentCityId",
                table: "CityEntity",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationEntity",
                table: "OrganizationEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CityEntity",
                table: "CityEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CityEntity_CityEntity_ParentCityId",
                table: "CityEntity",
                column: "ParentCityId",
                principalTable: "CityEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationEntity_OrganizationEntity_ParentId",
                table: "OrganizationEntity",
                column: "ParentId",
                principalTable: "OrganizationEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CityEntity_CityId",
                table: "Users",
                column: "CityId",
                principalTable: "CityEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_OrganizationEntity_OrganizationId",
                table: "Users",
                column: "OrganizationId",
                principalTable: "OrganizationEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
