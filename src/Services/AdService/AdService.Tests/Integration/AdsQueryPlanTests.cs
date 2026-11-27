using Npgsql;
using Xunit;
using Xunit.Abstractions;

namespace AdService.Tests.Integration;

public class AdsQueryPlanTests(AdsApi fixture, ITestOutputHelper output) : IClassFixture<AdsApi>
{
    [ExplainFact]
    public async Task ExplainCountAndPageQueriesWithAndWithoutOrderingIndex()
    {
        await using var connection = new NpgsqlConnection(fixture.ConnectionString);
        await connection.OpenAsync();
        await using (var seed = new NpgsqlCommand("""
            CREATE TEMP TABLE sample_ads AS
            SELECT md5(n::text)::uuid AS id, n, TIMESTAMPTZ '2026-02-01' + n * INTERVAL '1 second' AS created_at
            FROM generate_series(1, 50000) AS n;

            INSERT INTO "Ads" ("Id", "CreatedAt", "UpdatedAt", "CreatedBy", "UpdatedBy", "Status", "Views")
            SELECT id, created_at, created_at, 'EXPLAIN fixture', 'EXPLAIN fixture',
                CASE WHEN n % 5 = 0 THEN 'DRAFT' ELSE 'PUBLISHED' END, 0
            FROM sample_ads;

            INSERT INTO "SellerSnapshots" ("AdId", "SellerId", "ImageId", "RegistrationDate")
            SELECT id, CASE WHEN n % 101 = 0 THEN @owner ELSE md5((n % 101)::text)::uuid END, id, created_at
            FROM sample_ads;

            INSERT INTO "AdImage" ("Id", "AdId")
            SELECT id, id FROM sample_ads WHERE n % 2 = 0;

            DROP INDEX IF EXISTS "IX_Ads_CreatedAt_Id";
            ANALYZE;
            """, connection))
        {
            seed.Parameters.AddWithValue("owner", fixture.Owner);
            await seed.ExecuteNonQueryAsync();
        }

        await ExplainAsync(connection, "Baseline");

        await using var createIndex =
            new NpgsqlCommand("""CREATE INDEX "IX_Ads_CreatedAt_Id" ON "Ads" ("CreatedAt", "Id")""", connection);
        await createIndex.ExecuteNonQueryAsync();
        await ExplainAsync(connection, "Ordering index");
    }

    private async Task ExplainAsync(NpgsqlConnection connection, string variant)
    {
        (string Label, string Query)[] scenarios =
        [
            ("First page", "pageSize=20&includeImageUrl=false"),
            ("Deep page", "pageSize=20&pageNumber=1000&includeImageUrl=false"),
            ("Seller filter", $"pageSize=20&sellerId={fixture.Owner}&includeImageUrl=false")
        ];

        foreach (var (label, query) in scenarios)
        {
            fixture.Commands.Queries.Clear();
            await fixture.GetPageAsync(query);
            foreach (var (statement, index) in fixture.Commands.Queries.Select((statement, index) => (statement, index)))
            {
                await using var explain = new NpgsqlCommand($"EXPLAIN (ANALYZE, BUFFERS) {statement.Sql}", connection);
                explain.Parameters.AddRange(statement.Parameters);

                // Execute once to warm up buffers, then report a comparable repeat execution.
                await explain.ExecuteNonQueryAsync();
                await using var reader = await explain.ExecuteReaderAsync();
                output.WriteLine($"{variant} | {label} | {(index == 0 ? "Count" : "Page")}");
                while (await reader.ReadAsync())
                    output.WriteLine(reader.GetString(0));
            }
        }
    }

    private sealed class ExplainFactAttribute : FactAttribute
    {
        public ExplainFactAttribute()
        {
            if (Environment.GetEnvironmentVariable("AD_SERVICE_EXPLAIN") != "1" ||
                string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(AdsApi.ConnectionSetting)))
            {
                Skip = $"Set AD_SERVICE_EXPLAIN=1 and {AdsApi.ConnectionSetting} to collect query plans.";
            }
        }
    }
}
