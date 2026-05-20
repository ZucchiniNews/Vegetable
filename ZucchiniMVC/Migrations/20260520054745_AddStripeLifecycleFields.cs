using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zucchinimvc.Migrations
{
    /// <inheritdoc />
    public partial class AddStripeLifecycleFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CancelAtPeriodEnd",
                table: "UserSubscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledAt",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentPeriodEnd",
                table: "UserSubscriptions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelAtPeriodEnd",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "CancelledAt",
                table: "UserSubscriptions");

            migrationBuilder.DropColumn(
                name: "CurrentPeriodEnd",
                table: "UserSubscriptions");
        }
    }
}
