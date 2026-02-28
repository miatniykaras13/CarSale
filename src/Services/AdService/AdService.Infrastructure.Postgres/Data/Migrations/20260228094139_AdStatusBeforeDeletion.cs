using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdStatusBeforeDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatusBeforeDeletion",
                table: "Ads",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatusBeforeDeletion",
                table: "Ads",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
