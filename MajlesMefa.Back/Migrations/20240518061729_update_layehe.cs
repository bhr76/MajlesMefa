using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class update_layehe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarresiKollyatDate",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "ChekideMehvar",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "Commission",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "EblaghBeGovernment",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "EblaghBeVozara",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "IsConfirmedInNatijeJalasatCommsion",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NahveBarresi",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NatijeBarresiKollyatVoteNCount",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NatijeBarresiKollyatVotePCount",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NatijeBarresiMavadVoteNCount",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NatijeBarresiMavadVotePCount",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NazarNamayandeghan",
                table: "Layehes");

            migrationBuilder.RenameColumn(
                name: "TaghdimBeMajlesDate",
                table: "Layehes",
                newName: "ElamVosoolDate");

            migrationBuilder.RenameColumn(
                name: "NatijeBarresiMavad",
                table: "Layehes",
                newName: "VazeyatBarresi");

            migrationBuilder.RenameColumn(
                name: "NatijeBarresiKollyat",
                table: "Layehes",
                newName: "NatijeBarresiSahn");

            migrationBuilder.RenameColumn(
                name: "JalasateBarresiDate",
                table: "Layehes",
                newName: "EblaghDate");

            migrationBuilder.RenameColumn(
                name: "ErjaBeShoraDate",
                table: "Layehes",
                newName: "BaresiKoliatDarSahnDate");

            migrationBuilder.AddColumn<string>(
                name: "MajorCommissions",
                table: "Layehes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinorCommissions",
                table: "Layehes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "NatijeBarresiCommission",
                table: "Layehes",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "ShomareSabt",
                table: "Layehes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Type",
                table: "Layehes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MajorCommissions",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "MinorCommissions",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "NatijeBarresiCommission",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "ShomareSabt",
                table: "Layehes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Layehes");

            migrationBuilder.RenameColumn(
                name: "VazeyatBarresi",
                table: "Layehes",
                newName: "NatijeBarresiMavad");

            migrationBuilder.RenameColumn(
                name: "NatijeBarresiSahn",
                table: "Layehes",
                newName: "NatijeBarresiKollyat");

            migrationBuilder.RenameColumn(
                name: "ElamVosoolDate",
                table: "Layehes",
                newName: "TaghdimBeMajlesDate");

            migrationBuilder.RenameColumn(
                name: "EblaghDate",
                table: "Layehes",
                newName: "JalasateBarresiDate");

            migrationBuilder.RenameColumn(
                name: "BaresiKoliatDarSahnDate",
                table: "Layehes",
                newName: "ErjaBeShoraDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "BarresiKollyatDate",
                table: "Layehes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChekideMehvar",
                table: "Layehes",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Commission",
                table: "Layehes",
                type: "uniqueidentifier",
                maxLength: 64,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EblaghBeGovernment",
                table: "Layehes",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EblaghBeVozara",
                table: "Layehes",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmedInNatijeJalasatCommsion",
                table: "Layehes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NahveBarresi",
                table: "Layehes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NatijeBarresiKollyatVoteNCount",
                table: "Layehes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NatijeBarresiKollyatVotePCount",
                table: "Layehes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NatijeBarresiMavadVoteNCount",
                table: "Layehes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NatijeBarresiMavadVotePCount",
                table: "Layehes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NazarNamayandeghan",
                table: "Layehes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");
        }
    }
}
