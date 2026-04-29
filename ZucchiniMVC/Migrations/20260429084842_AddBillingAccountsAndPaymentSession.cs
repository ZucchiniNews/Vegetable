using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zucchinimvc.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingAccountsAndPaymentSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviderPriceId",
                table: "Plans",
                newName: "StripePriceId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivatedAt",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAccounts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingAccounts");

            migrationBuilder.DropColumn(
                name: "ActivatedAt",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserSubscriptions");

            migrationBuilder.RenameColumn(
                name: "StripePriceId",
                table: "Plans",
                newName: "ProviderPriceId");
        }
    }
}
