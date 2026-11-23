using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfileService.Infrastructure.Postgres.Data.Migrations
{
    /// <inheritdoc />
    public partial class KeycloakIdAsGuid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "UserProfiles"
                ALTER COLUMN "KeycloakId" TYPE uuid
                USING "KeycloakId"::uuid;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "UserProfiles"
                ALTER COLUMN "KeycloakId" TYPE character varying(255)
                USING "KeycloakId"::text;
                """);
        }
    }
}
