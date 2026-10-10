using System;
using MajlesMefa.Back.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajlesMefa.Back.Migrations
{
    [DbContext(typeof(RefahMajlesDbContext))]
    [Migration("20261001090000_AddSenatorAnnualLoanBudgets")]
    public partial class AddSenatorAnnualLoanBudgets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SenatorAnnualLoanBudgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    GharzolHasaneLimit = table.Column<long>(type: "bigint", nullable: false),
                    MorabeheLimit = table.Column<long>(type: "bigint", nullable: false),
                    GharzolHasaneUsed = table.Column<long>(type: "bigint", nullable: false),
                    MorabeheUsed = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    CreatorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SenatorAnnualLoanBudgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SenatorAnnualLoanBudgets_SenatorProfiles_SenatorId",
                        column: x => x.SenatorId,
                        principalTable: "SenatorProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SenatorAnnualLoanBudgets_Created",
                table: "SenatorAnnualLoanBudgets",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_SenatorAnnualLoanBudgets_SenatorId_Year",
                table: "SenatorAnnualLoanBudgets",
                columns: new[] { "SenatorId", "Year" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SenatorAnnualLoanBudgets");
        }

        protected override void BuildTargetModel(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);
        }
    }
}
