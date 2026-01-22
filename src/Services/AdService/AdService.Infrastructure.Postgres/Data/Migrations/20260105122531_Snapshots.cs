using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class Snapshots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "DriveType",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "CarSnapshots");

            migrationBuilder.RenameColumn(
                name: "HorsePower",
                table: "CarSnapshots",
                newName: "Engine_HorsePower");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "CarSnapshots",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Consumption",
                table: "CarSnapshots",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BodyType_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyType_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Brand_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DriveType_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriveType_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Engine_FuelType_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Engine_FuelType_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Engine_GenerationId",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Engine_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Engine_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generation_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generation_ModelId",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Generation_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generation_YearFrom",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Generation_YearTo",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Model_BrandId",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Model_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransmissionType_Id",
                table: "CarSnapshots",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType_Name",
                table: "CarSnapshots",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Price_CurrencyCode",
                table: "AdPrices",
                type: "text",
                nullable: false,
                defaultValue: "USD",
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyType_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "BodyType_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Brand_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Brand_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "DriveType_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "DriveType_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Engine_FuelType_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Engine_FuelType_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Engine_GenerationId",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Engine_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Engine_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation_ModelId",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation_YearFrom",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Generation_YearTo",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Model_BrandId",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Model_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "Model_Name",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "TransmissionType_Id",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "TransmissionType_Name",
                table: "CarSnapshots");

            migrationBuilder.RenameColumn(
                name: "Engine_HorsePower",
                table: "CarSnapshots",
                newName: "HorsePower");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "CarSnapshots",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Consumption",
                table: "CarSnapshots",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BodyType",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriveType",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Generation",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Price_CurrencyCode",
                table: "AdPrices",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "USD");
        }
    }
}
