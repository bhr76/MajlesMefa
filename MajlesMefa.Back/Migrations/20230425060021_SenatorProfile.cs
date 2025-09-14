using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class SenatorProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarFileName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValueSql: "1");

            migrationBuilder.CreateTable(
                name: "SenatorProfiles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Mobile = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    ComissionMembership = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    SocailActivity = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    SenaHistory = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    JobHistory = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PoliticalTending = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    PersonalFavorites = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    HozeCityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthCityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenatorProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_SenatorProfiles_Cities_BirthCityId",
                        column: x => x.BirthCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenatorProfiles_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenatorProfiles_Cities_HozeCityId",
                        column: x => x.HozeCityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SenatorProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SenatorProfiles_BirthCityId",
                table: "SenatorProfiles",
                column: "BirthCityId");

            migrationBuilder.CreateIndex(
                name: "IX_SenatorProfiles_CityId",
                table: "SenatorProfiles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SenatorProfiles_HozeCityId",
                table: "SenatorProfiles",
                column: "HozeCityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SenatorProfiles");

            migrationBuilder.DropColumn(
                name: "AvatarFileName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Users");
        }
    }
}
