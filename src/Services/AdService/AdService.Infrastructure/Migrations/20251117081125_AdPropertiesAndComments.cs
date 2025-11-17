using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AdService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdPropertiesAndComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seller_DisplayName",
                table: "Ads",
                newName: "Seller_Name");

            migrationBuilder.RenameColumn(
                name: "ModerationResult_ModeratorId",
                table: "Ads",
                newName: "ModeratorId");

            migrationBuilder.RenameColumn(
                name: "ModerationResult_Message",
                table: "Ads",
                newName: "ModeratorMessage");

            migrationBuilder.RenameColumn(
                name: "ModerationResult_DenyReason",
                table: "Ads",
                newName: "DenyReason");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Ads",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "Ads",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<HashSet<Guid>>(
                name: "Images",
                table: "Ads",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AdId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Ads_AdId",
                        column: x => x.AdId,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AdId",
                table: "Comment",
                column: "AdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Ads");

            migrationBuilder.RenameColumn(
                name: "Seller_Name",
                table: "Ads",
                newName: "Seller_DisplayName");

            migrationBuilder.RenameColumn(
                name: "ModeratorMessage",
                table: "Ads",
                newName: "ModerationResult_Message");

            migrationBuilder.RenameColumn(
                name: "ModeratorId",
                table: "Ads",
                newName: "ModerationResult_ModeratorId");

            migrationBuilder.RenameColumn(
                name: "DenyReason",
                table: "Ads",
                newName: "ModerationResult_DenyReason");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Ads",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CarId",
                table: "Ads",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
