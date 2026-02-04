using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileManagement.Grpc.Data.Migrations
{
    /// <inheritdoc />
    public partial class BucketName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BucketName",
                table: "Files",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BucketName",
                table: "Files");
        }
    }
}
