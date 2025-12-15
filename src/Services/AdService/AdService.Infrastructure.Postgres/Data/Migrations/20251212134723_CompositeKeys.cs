using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class CompositeKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarSnapshots_Ads_CarId",
                table: "CarSnapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerSnapshots_Ads_SellerId",
                table: "SellerSnapshots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerSnapshots",
                table: "SellerSnapshots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarSnapshots",
                table: "CarSnapshots");

            migrationBuilder.AddColumn<Guid>(
                name: "AdId",
                table: "SellerSnapshots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.AddColumn<Guid>(
                name: "AdId",
                table: "CarSnapshots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerSnapshots",
                table: "SellerSnapshots",
                columns: new[] { "AdId", "SellerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarSnapshots",
                table: "CarSnapshots",
                columns: new[] { "AdId", "CarId" });

            migrationBuilder.CreateIndex(
                name: "IX_SellerSnapshots_AdId",
                table: "SellerSnapshots",
                column: "AdId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarSnapshots_AdId",
                table: "CarSnapshots",
                column: "AdId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarSnapshots_Ads_AdId",
                table: "CarSnapshots",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerSnapshots_Ads_AdId",
                table: "SellerSnapshots",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarSnapshots_Ads_AdId",
                table: "CarSnapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SellerSnapshots_Ads_AdId",
                table: "SellerSnapshots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SellerSnapshots",
                table: "SellerSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_SellerSnapshots_AdId",
                table: "SellerSnapshots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarSnapshots",
                table: "CarSnapshots");

            migrationBuilder.DropIndex(
                name: "IX_CarSnapshots_AdId",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "SellerSnapshots");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "CarSnapshots");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_SellerSnapshots",
                table: "SellerSnapshots",
                column: "SellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarSnapshots",
                table: "CarSnapshots",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarSnapshots_Ads_CarId",
                table: "CarSnapshots",
                column: "CarId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellerSnapshots_Ads_SellerId",
                table: "SellerSnapshots",
                column: "SellerId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
