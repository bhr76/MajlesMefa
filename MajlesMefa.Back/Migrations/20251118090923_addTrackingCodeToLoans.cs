using System;
using Microsoft.EntityFrameworkCore.Migrations;


namespace MajlesMefa.Back.Migrations
{
    public partial class addTrackingCodeToLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // اضافه کردن ستون با Default Value
            migrationBuilder.AddColumn<int>(
                name: "TrackingCode",
                table: "Loans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // مقداردهی اولیه برای رکوردهای موجود
            migrationBuilder.Sql(@"
                DECLARE @Counter INT = 1000;
                
                WITH OrderedLoans AS (
                    SELECT 
                        L.DataEntryId,
                        ROW_NUMBER() OVER (ORDER BY DE.Created) AS RowNum
                    FROM Loans L
                    INNER JOIN DataEntries DE ON L.DataEntryId = DE.Id
                )
                UPDATE L
                SET L.TrackingCode = OL.RowNum + @Counter - 1
                FROM Loans L
                INNER JOIN OrderedLoans OL ON L.DataEntryId = OL.DataEntryId;
            ");

            // حذف Default Value
            migrationBuilder.AlterColumn<int>(
                name: "TrackingCode",
                table: "Loans",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: false,
                oldDefaultValue: 0);

            // اضافه کردن Unique Index
            migrationBuilder.CreateIndex(
                name: "IX_Loans_TrackingCode",
                table: "Loans",
                column: "TrackingCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loans_TrackingCode",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Loans");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Loans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}