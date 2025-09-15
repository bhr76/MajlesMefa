using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_tahghighTafahosSenators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TahghighTafahosSenators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TahghighTafahosId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TahghighTafahosSenators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TahghighTafahosSenators_SenatorProfiles_SenatorId",
                        column: x => x.SenatorId,
                        principalTable: "SenatorProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TahghighTafahosSenators_TahghighTafahoses_TahghighTafahosId",
                        column: x => x.TahghighTafahosId,
                        principalTable: "TahghighTafahoses",
                        principalColumn: "DataEntryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TahghighTafahosSenators_SenatorId",
                table: "TahghighTafahosSenators",
                column: "SenatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TahghighTafahosSenators_TahghighTafahosId",
                table: "TahghighTafahosSenators",
                column: "TahghighTafahosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TahghighTafahosSenators");
        }
    }
}
