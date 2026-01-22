using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class RewritingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Engine_Generation_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generation_Model_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Model_Brand_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Engine_Generation_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation_Model_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Model_Brand_Id",
                table: "CarSnapshots");
        }
    }
}
