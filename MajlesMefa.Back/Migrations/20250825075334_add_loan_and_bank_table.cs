using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    /// <inheritdoc />
    public partial class add_loan_and_bank_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    DataEntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SuggestedBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanType = table.Column<byte>(type: "tinyint", nullable: false),
                    VaziatPasokh = table.Column<byte>(type: "tinyint", nullable: false),
                    PasokhNo = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    PasokhDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    NationalNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.DataEntryId);
                    table.ForeignKey(
                        name: "FK_Loans_Banks_RelatedBankId",
                        column: x => x.RelatedBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_Banks_SuggestedBankId",
                        column: x => x.SuggestedBankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loans_DataEntries_DataEntryId",
                        column: x => x.DataEntryId,
                        principalTable: "DataEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banks_Created",
                table: "Banks",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_RelatedBankId",
                table: "Loans",
                column: "RelatedBankId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_SuggestedBankId",
                table: "Loans",
                column: "SuggestedBankId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
