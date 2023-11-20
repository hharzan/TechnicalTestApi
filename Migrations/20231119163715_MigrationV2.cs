using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnicalTestAPI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receivable_Debtor_DebtorId",
                table: "Receivable");

            migrationBuilder.DropIndex(
                name: "IX_Receivable_DebtorId",
                table: "Receivable");

            migrationBuilder.AlterColumn<string>(
                name: "IssueDate",
                table: "Receivable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DueDate",
                table: "Receivable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "DebtorId",
                table: "Receivable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "ClosedDate",
                table: "Receivable",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DebtorCountryCode",
                table: "Receivable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DebtorName",
                table: "Receivable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebtorCountryCode",
                table: "Receivable");

            migrationBuilder.DropColumn(
                name: "DebtorName",
                table: "Receivable");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssueDate",
                table: "Receivable",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Receivable",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "DebtorId",
                table: "Receivable",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ClosedDate",
                table: "Receivable",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receivable_DebtorId",
                table: "Receivable",
                column: "DebtorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receivable_Debtor_DebtorId",
                table: "Receivable",
                column: "DebtorId",
                principalTable: "Debtor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
