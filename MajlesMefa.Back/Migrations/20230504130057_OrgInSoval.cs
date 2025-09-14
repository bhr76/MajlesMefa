using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class OrgInSoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Organization",
                table: "Sovals",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "SenatorProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FractionMembership",
                table: "SenatorProfiles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "GerayeshSiasi",
                table: "SenatorProfiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "HozeEntekhabi",
                table: "SenatorProfiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "MadrakTahsili",
                table: "SenatorProfiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "MahaleTahsil",
                table: "SenatorProfiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Reshte",
                table: "SenatorProfiles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SabegheEmzaEstizah",
                table: "SenatorProfiles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SabegheHeyatReise",
                table: "SenatorProfiles",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "ShoghleGhaleb",
                table: "SenatorProfiles",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Organization",
                table: "Sovals");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "FractionMembership",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "GerayeshSiasi",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "HozeEntekhabi",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "MadrakTahsili",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "MahaleTahsil",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "Reshte",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "SabegheEmzaEstizah",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "SabegheHeyatReise",
                table: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "ShoghleGhaleb",
                table: "SenatorProfiles");
        }
    }
}
