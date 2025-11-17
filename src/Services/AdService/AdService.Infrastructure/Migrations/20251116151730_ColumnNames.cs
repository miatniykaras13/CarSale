using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SellerName",
                table: "Ads",
                newName: "Seller_DisplayName");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "Ads",
                newName: "Price_CurrencyCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seller_DisplayName",
                table: "Ads",
                newName: "SellerName");

            migrationBuilder.RenameColumn(
                name: "Price_CurrencyCode",
                table: "Ads",
                newName: "CurrencyCode");
        }
    }
}
