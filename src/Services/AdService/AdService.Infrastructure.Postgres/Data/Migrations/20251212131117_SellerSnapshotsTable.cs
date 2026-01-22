using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class SellerSnapshotsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarSnapshots_Ads_AdId",
                table: "CarSnapshots");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Seller_ImageId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Seller_Name",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "Seller_RegistrationDate",
                table: "Ads");

            migrationBuilder.RenameColumn(
                name: "AdId",
                table: "CarSnapshots",
                newName: "CarId");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Generation",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "CarSnapshots",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "SellerSnapshots",
                columns: table => new
                {
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerSnapshots", x => x.SellerId);
                    table.ForeignKey(
                        name: "FK_SellerSnapshots_Ads_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_CarSnapshots_Ads_CarId",
                table: "CarSnapshots",
                column: "CarId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarSnapshots_Ads_CarId",
                table: "CarSnapshots");

            migrationBuilder.DropTable(
                name: "SellerSnapshots");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "CarSnapshots",
                newName: "AdId");

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "CarSnapshots",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Generation",
                table: "CarSnapshots",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "CarSnapshots",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "CarId",
                table: "Ads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SellerId",
                table: "Ads",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Seller_ImageId",
                table: "Ads",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Seller_Name",
                table: "Ads",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Seller_RegistrationDate",
                table: "Ads",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarSnapshots_Ads_AdId",
                table: "CarSnapshots",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
