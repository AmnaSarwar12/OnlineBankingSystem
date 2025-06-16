using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineBankingSystem.Migrations
{
    /// <inheritdoc />
    public partial class seedBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "bankAccounts",
                columns: new[] { "id", "AccountNumber", "AccountType", "Balance", "OpenedOn", "Status", "userId" },
                values: new object[] { 1, "123456789", "Savings", 5000.00m, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 1 });

            migrationBuilder.InsertData(
                table: "Transaction",
                columns: new[] { "Id", "Amount", "BankAccountId", "Description", "TimeStamp", "Type" },
                values: new object[,]
                {
                    { 1, 150.0m, 1, "my first transaction", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deposit" },
                    { 2, 150.0m, 1, "my Second transaction", new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Withdraw" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transaction",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Transaction",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "bankAccounts",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
