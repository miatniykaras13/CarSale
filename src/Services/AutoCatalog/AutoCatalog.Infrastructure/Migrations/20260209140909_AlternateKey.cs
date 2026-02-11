using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlternateKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_BrandId",
                table: "Cars");

            migrationBuilder.AlterColumn<Guid>(
                name: "PhotoId",
                table: "Cars",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cars_BrandId_ModelId_GenerationId_EngineId_BodyTypeId_Drive~",
                table: "Cars",
                columns: new[] { "BrandId", "ModelId", "GenerationId", "EngineId", "BodyTypeId", "DriveTypeId", "TransmissionTypeId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cars_BrandId_ModelId_GenerationId_EngineId_BodyTypeId_Drive~",
                table: "Cars");

            migrationBuilder.AlterColumn<Guid>(
                name: "PhotoId",
                table: "Cars",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_BrandId",
                table: "Cars",
                column: "BrandId");
        }
    }
}
