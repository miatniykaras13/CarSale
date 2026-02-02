using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnumsBecameEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelType",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "DriveType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "YearTo",
                table: "Cars",
                newName: "TransmissionTypeId");

            migrationBuilder.RenameColumn(
                name: "YearFrom",
                table: "Cars",
                newName: "DriveTypeId");

            migrationBuilder.AddColumn<int>(
                name: "YearFrom",
                table: "Generations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearTo",
                table: "Generations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FuelTypeId",
                table: "Engines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<float>(
                name: "Consumption",
                table: "Cars",
                type: "real",
                precision: 18,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldPrecision: 18,
                oldScale: 1);

            migrationBuilder.AlterColumn<float>(
                name: "Acceleration",
                table: "Cars",
                type: "real",
                precision: 18,
                scale: 1,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,1)",
                oldPrecision: 18,
                oldScale: 1);

            migrationBuilder.AddColumn<int>(
                name: "BodyTypeId",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "YearTo",
                table: "Brands",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "BodyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FuelTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransmissionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransmissionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Engines_FuelTypeId",
                table: "Engines",
                column: "FuelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BodyTypeId",
                table: "Cars",
                column: "BodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriveTypeId",
                table: "Cars",
                column: "DriveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TransmissionTypeId",
                table: "Cars",
                column: "TransmissionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_BodyTypes_BodyTypeId",
                table: "Cars",
                column: "BodyTypeId",
                principalTable: "BodyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_DriveTypes_DriveTypeId",
                table: "Cars",
                column: "DriveTypeId",
                principalTable: "DriveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_TransmissionTypes_TransmissionTypeId",
                table: "Cars",
                column: "TransmissionTypeId",
                principalTable: "TransmissionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_FuelTypes_FuelTypeId",
                table: "Engines",
                column: "FuelTypeId",
                principalTable: "FuelTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_BodyTypes_BodyTypeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_DriveTypes_DriveTypeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Cars_TransmissionTypes_TransmissionTypeId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Engines_FuelTypes_FuelTypeId",
                table: "Engines");

            migrationBuilder.DropTable(
                name: "BodyTypes");

            migrationBuilder.DropTable(
                name: "DriveTypes");

            migrationBuilder.DropTable(
                name: "FuelTypes");

            migrationBuilder.DropTable(
                name: "TransmissionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Engines_FuelTypeId",
                table: "Engines");

            migrationBuilder.DropIndex(
                name: "IX_Cars_BodyTypeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_DriveTypeId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_TransmissionTypeId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "YearFrom",
                table: "Generations");

            migrationBuilder.DropColumn(
                name: "YearTo",
                table: "Generations");

            migrationBuilder.DropColumn(
                name: "FuelTypeId",
                table: "Engines");

            migrationBuilder.DropColumn(
                name: "BodyTypeId",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "TransmissionTypeId",
                table: "Cars",
                newName: "YearTo");

            migrationBuilder.RenameColumn(
                name: "DriveTypeId",
                table: "Cars",
                newName: "YearFrom");

            migrationBuilder.AddColumn<string>(
                name: "FuelType",
                table: "Engines",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Consumption",
                table: "Cars",
                type: "numeric(18,1)",
                precision: 18,
                scale: 1,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldPrecision: 18,
                oldScale: 1);

            migrationBuilder.AlterColumn<decimal>(
                name: "Acceleration",
                table: "Cars",
                type: "numeric(18,1)",
                precision: 18,
                scale: 1,
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldPrecision: 18,
                oldScale: 1);

            migrationBuilder.AddColumn<string>(
                name: "DriveType",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransmissionType",
                table: "Cars",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "YearTo",
                table: "Brands",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
