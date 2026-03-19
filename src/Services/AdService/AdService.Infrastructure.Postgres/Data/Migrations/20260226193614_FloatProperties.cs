using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class FloatProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Consumption",
                table: "CarSnapshots",
                type: "real",
                precision: 18,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldPrecision: 18,
                oldScale: 1,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Consumption",
                table: "CarSnapshots",
                type: "numeric(18,1)",
                precision: 18,
                scale: 1,
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldPrecision: 18,
                oldScale: 1,
                oldNullable: true);
        }
    }
}
