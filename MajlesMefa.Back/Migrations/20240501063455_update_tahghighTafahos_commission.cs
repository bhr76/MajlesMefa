using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class update_tahghighTafahos_commission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TahghighTafahoses_Commission",
                table: "TahghighTafahoses",
                column: "Commission");

            migrationBuilder.AddForeignKey(
                name: "FK_TahghighTafahoses_DastoorJalasatComissions_Commission",
                table: "TahghighTafahoses",
                column: "Commission",
                principalTable: "DastoorJalasatComissions",
                principalColumn: "DataEntryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TahghighTafahoses_DastoorJalasatComissions_Commission",
                table: "TahghighTafahoses");

            migrationBuilder.DropIndex(
                name: "IX_TahghighTafahoses_Commission",
                table: "TahghighTafahoses");
        }
    }
}
