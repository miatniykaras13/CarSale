using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KeycloakId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email_Value = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PhoneNumber_E164 = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdSnapshots",
                columns: table => new
                {
                    AdId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Car_Brand = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Car_Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Car_Generation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Car_Year = table.Column<int>(type: "integer", nullable: true),
                    Car_DriveType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Car_TransmissionType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Car_EngineVolume = table.Column<double>(type: "double precision", precision: 18, scale: 1, nullable: true),
                    Car_FuelType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Car_BodyType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Price_CurrencyCode = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    Price_Amount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdSnapshots", x => new { x.UserProfileId, x.AdId });
                    table.ForeignKey(
                        name: "FK_AdSnapshots_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_KeycloakId",
                table: "UserProfiles",
                column: "KeycloakId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_Username",
                table: "UserProfiles",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdSnapshots");

            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}
