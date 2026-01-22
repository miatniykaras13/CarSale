using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Precision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Consumption",
                table: "Cars",
                type: "numeric(18,1)",
                precision: 18,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Acceleration",
                table: "Cars",
                type: "numeric(18,1)",
                precision: 18,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Consumption",
                table: "Cars",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldPrecision: 18,
                oldScale: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "Acceleration",
                table: "Cars",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldPrecision: 18,
                oldScale: 1);
        }
    }
}
