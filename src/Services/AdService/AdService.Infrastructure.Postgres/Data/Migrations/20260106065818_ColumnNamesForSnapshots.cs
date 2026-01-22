using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class ColumnNamesForSnapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType_Name1",
                table: "CarSnapshots");

            migrationBuilder.AlterColumn<string>(
                name: "BodyType_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodyType_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType_Id",
                table: "CarSnapshots");

            migrationBuilder.AlterColumn<int>(
                name: "BodyType_Name",
                table: "CarSnapshots",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyType_Name1",
                table: "CarSnapshots",
                type: "text",
                nullable: true);
        }
    }
}
