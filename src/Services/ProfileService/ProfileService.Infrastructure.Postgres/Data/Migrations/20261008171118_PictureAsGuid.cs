using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class PictureAsGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                    ALTER TABLE "UserProfiles"
                    ALTER COLUMN "Picture" TYPE uuid
                    USING "Picture"::uuid;
                    """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                     ALTER TABLE "UserProfiles"
                     ALTER COLUMN "Picture" TYPE character varying(255)
                     USING "Picture"::text;
                     """);
        }
    }
}
