using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BrandCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Brands");
        }
    }
}
