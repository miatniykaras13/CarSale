using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class NullableProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationExists",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Location_City",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Location_Region",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "PriceExists",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Price_Amount",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Price_CurrencyCode",
                table: "Ads");

            migrationBuilder.CreateTable(
                name: "AdLocations",
                columns: table => new
                {
                    AdId = table.Column<Guid>(type: "uuid", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdLocations", x => x.AdId);
                    table.ForeignKey(
                        name: "FK_AdLocations_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdPrices",
                columns: table => new
                {
                    AdId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price_CurrencyCode = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdPrices", x => x.AdId);
                    table.ForeignKey(
                        name: "FK_AdPrices_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdLocations");

            migrationBuilder.DropTable(
                name: "AdPrices");

            migrationBuilder.AddColumn<bool>(
                name: "LocationExists",
                table: "Ads",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location_City",
                table: "Ads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location_Region",
                table: "Ads",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PriceExists",
                table: "Ads",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Price_Amount",
                table: "Ads",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price_CurrencyCode",
                table: "Ads",
                type: "text",
                nullable: true);
        }
    }
}
