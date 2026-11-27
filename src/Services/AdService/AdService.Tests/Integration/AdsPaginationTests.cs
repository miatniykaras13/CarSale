using System.Collections.Concurrent;
using System.Data.Common;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using AdService.Application;
using AdService.Application.Abstractions.Data;
using AdService.Application.Abstractions.FileStorage;
using AdService.Contracts.Ads.ListItems;
using AdService.Contracts.Files;
using AdService.Domain.Aggregates;
using AdService.Domain.Enums;
using AdService.Domain.ValueObjects;
using AdService.Infrastructure.Postgres.Data;
using AdService.Presenters.Endpoints.Get;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Xunit;

namespace AdService.Tests.Integration;

public class AdsPaginationTests(AdsApi fixture) : IClassFixture<AdsApi>
{
    [PostgresTheory]
    [InlineData(null, 4)]
    [InlineData("owner", 10)]
    [InlineData("other", 5)]
    public async Task VisibilityAndSellerFilterAreAppliedBeforePaging(string? caller, int visibleCount)
    {
        var seller = caller switch
        {
            "owner" => fixture.Owner,
            "other" => fixture.Other,
            _ => (Guid?)null
        };

        var page = await fixture.GetPageAsync("pageSize=2", seller);
        Assert.Equal(visibleCount, page.TotalCount);
        Assert.Equal(2, page.Items.Count);

        var filtered = await fixture.GetPageAsync($"sellerId={fixture.Owner}&pageSize=2", seller);
        var statuses = filtered.Items.Select(item => item.Status).ToArray();
        Assert.Equal(caller == "owner" ? 9 : 3, filtered.TotalCount);
        Assert.Equal(caller == "owner" ? ["SOLD", "EXPIRED"] : new[] { "SOLD", "ARCHIVED" }, statuses);
        Assert.All(filtered.Items, item => Assert.Equal(fixture.Owner, item.Seller.SellerId));
    }

    [PostgresTheory]
    [InlineData("newest")]
    [InlineData("oldest")]
    public async Task TiedTimestampsHaveStableOrderAcrossPageBoundaries(string sort)
    {
        var expected = fixture.PublicAds.OrderBy(ad => ad.CreatedAt).ThenBy(ad => ad.Id).Select(ad => ad.Id);
        var orderedIds = (sort == "newest" ? expected.Reverse() : expected).ToArray();

        var first = await fixture.GetPageAsync($"sort={sort}&pageSize=2");
        var second = await fixture.GetPageAsync($"sort={sort}&pageSize=2&pageNumber=2");
        var last = await fixture.GetPageAsync($"sort={sort}&pageSize=3&pageNumber=2");
        var empty = await fixture.GetPageAsync($"sort={sort}&pageSize=3&pageNumber=3");

        Assert.Equal(orderedIds[..2], first.Items.Select(ad => ad.AdId));
        Assert.Equal(orderedIds[2..], second.Items.Select(ad => ad.AdId));
        Assert.Equal(orderedIds[3..], last.Items.Select(ad => ad.AdId));
        Assert.Empty(empty.Items);
        Assert.Equal(4, last.TotalCount);
        Assert.Equal(4, empty.TotalCount);
        Assert.Equal(3, empty.PageNumber);
        Assert.Equal(3, empty.PageSize);
    }

    [PostgresTheory]
    [InlineData("pageNumber=0")]
    [InlineData("pageSize=0")]
    [InlineData("pageSize=101")]
    [InlineData("pageNumber=2147483647&pageSize=100")]
    [InlineData("sort=price")]
    public async Task InvalidParametersReturnBadRequestWithoutReadingDatabase(string query)
    {
        fixture.Commands.Queries.Clear();
        using var response = await fixture.Client.GetAsync($"/ads?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Empty(fixture.Commands.Queries);
    }

    [PostgresTheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task OnlyTheRequestedPageIsReadAndEnriched(bool includeImageUrl)
    {
        fixture.Commands.Queries.Clear();
        fixture.ImageRequests.Clear();
        var page = await fixture.GetPageAsync($"pageNumber=2&pageSize=2&includeImageUrl={includeImageUrl}");

        var queries = fixture.Commands.Queries.ToArray();
        Assert.Equal(2, queries.Length);
        Assert.Contains("count(*)", queries[0].Sql, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("LIMIT", queries[0].Sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ORDER BY", queries[1].Sql);
        Assert.Contains("\"CreatedAt\" DESC", queries[1].Sql);
        Assert.Contains("\"Id\" DESC", queries[1].Sql);
        Assert.Contains("LIMIT", queries[1].Sql);
        Assert.Contains("OFFSET", queries[1].Sql);
        Assert.All(queries, query => Assert.Equal(System.Data.IsolationLevel.RepeatableRead, query.Isolation));
        Assert.Equal(2, page.Items.Count);
        Assert.All(page.Items, ad => Assert.NotNull(ad.ImageId));
        Assert.All(page.Items, ad => Assert.Equal(includeImageUrl ? $"https://images.test/{ad.ImageId}" : null, ad.ImageUrl));
        Assert.Equal(
            includeImageUrl ? page.Items.Select(ad => ad.ImageId!.Value).Order().ToArray() : Array.Empty<Guid>(),
            fixture.ImageRequests.Order().ToArray());
    }

    [PostgresTheory]
    [InlineData("")]
    [InlineData("pageSize=100")]
    public async Task DefaultAndMaximumPageSizesAreAccepted(string query)
    {
        var page = await fixture.GetPageAsync(query);
        Assert.Equal(1, page.PageNumber);
        Assert.Equal(string.IsNullOrEmpty(query) ? 5 : 100, page.PageSize);
        Assert.Equal(4, page.TotalCount);
        Assert.Equal(
            fixture.PublicAds.OrderByDescending(ad => ad.CreatedAt).ThenByDescending(ad => ad.Id)
            .Select(ad => ad.Id), page.Items.Select(ad => ad.AdId));
    }
}

public sealed class AdsApi : IAsyncLifetime
{
    public const string ConnectionSetting = "AD_SERVICE_TEST_POSTGRES";
    private WebApplication? app;

    public Guid Owner { get; } = Guid.NewGuid();

    public Guid Other { get; } = Guid.NewGuid();

    public List<Ad> PublicAds { get; } = [];

    public ConcurrentQueue<Guid> ImageRequests { get; } = new();

    public SqlRecorder Commands { get; } = new();

    public HttpClient Client { get; private set; } = null!;

    public string? ConnectionString { get; private set; }

    public async Task InitializeAsync()
    {
        var baseConnectionString = Environment.GetEnvironmentVariable(ConnectionSetting);
        if (string.IsNullOrWhiteSpace(baseConnectionString))
            return;

        ConnectionString = new NpgsqlConnectionStringBuilder(baseConnectionString)
        {
            Database = $"ads_test_{Guid.NewGuid():N}",
        }.ConnectionString;

        await using (var db = CreateContext())
        {
            await db.Database.EnsureCreatedAsync();
            foreach (var status in Enum.GetValues<AdStatus>())
            {
                AddAd(db, Owner, status, (int)status);
            }

            AddAd(db, Other, AdStatus.DRAFT, 10);
            AddAd(db, Other, AdStatus.PUBLISHED, (int)AdStatus.ARCHIVED);

            await db.SaveChangesAsync();
        }

        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("http://127.0.0.1:0");
        builder.Logging.ClearProviders();
        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddScoped<IAppDbContext>(_ => CreateContext(Commands));
        builder.Services.AddSingleton<IFileStorage>(new RecordingFileStorage(ImageRequests));

        app = builder.Build();
        app.UseDeveloperExceptionPage();
        app.Use(async (context, next) =>
        {
            if (context.Request.Headers.TryGetValue("Test-User", out var user))
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.NameIdentifier, user.ToString())], "Test"));
            }

            await next(context);
        });
        new GetAds().AddRoutes(app);
        await app.StartAsync();
        Client = new HttpClient { BaseAddress = new Uri(app.Urls.Single()) };
    }

    public async Task<AdsPageDto> GetPageAsync(string query, Guid? userId = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/ads?{query}");
        if (userId.HasValue)
            request.Headers.Add("Test-User", userId.ToString());

        using var response = await Client.SendAsync(request);
        var body = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode, body);
        return (await response.Content.ReadFromJsonAsync<AdsPageDto>())!;
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();
        if (app is not null)
            await app.DisposeAsync();

        if (ConnectionString is not null)
        {
            await using var db = CreateContext();
            await db.Database.EnsureDeletedAsync();
        }
    }

    private AppDbContext CreateContext(SqlRecorder? recorder = null)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(ConnectionString);
        if (recorder is not null)
            builder.AddInterceptors(recorder);

        return new AppDbContext(builder.Options);
    }

    private void AddAd(AppDbContext db, Guid sellerId, AdStatus status, int ageOrder)
    {
        var date = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(ageOrder);
        var seller = SellerSnapshot.Of(sellerId, "Test seller", date, Guid.NewGuid(), null).Value;
        var ad = Ad.Create(seller).Value;
        ad.CreatedAt = date;
        ad.UpdatedAt = date;
        ad.UpdatedBy = "Test seller";
        Assert.True(ad.AddImages([AdImage.Of(Guid.NewGuid()).Value]).IsSuccess);
        db.Ads.Add(ad);
        db.Entry(ad).Property(a => a.Status).CurrentValue = status;
        if (status is AdStatus.PUBLISHED or AdStatus.ARCHIVED or AdStatus.SOLD)
            PublicAds.Add(ad);
    }

    private sealed class RecordingFileStorage(ConcurrentQueue<Guid> requests) : IFileStorage
    {
        public Task<string> GetDownloadLinkAsync(Guid fileId, int expirySeconds, CancellationToken ct)
        {
            requests.Enqueue(fileId);
            Assert.Equal(600, expirySeconds);
            return Task.FromResult($"https://images.test/{fileId}");
        }

        public Task<Guid> UploadFileAsync(Stream stream, string name, string contentType, CancellationToken ct) =>
            throw new NotSupportedException();

        public Task<bool> DeleteFileAsync(Guid fileId, CancellationToken ct) => throw new NotSupportedException();

        public Task<Guid> GenerateThumbnailAsync(Guid fileId, ThumbnailDto thumbnail, CancellationToken ct) =>
            throw new NotSupportedException();
    }
}

public sealed class SqlRecorder : DbCommandInterceptor
{
    public ConcurrentQueue<RecordedQuery> Queries { get; } = new();

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        var parameters = command.Parameters.Cast<NpgsqlParameter>().Select(parameter => parameter.Clone()).ToArray();
        Queries.Enqueue(new RecordedQuery(command.CommandText, command.Transaction?.IsolationLevel, parameters));
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public sealed record RecordedQuery(
        string Sql, System.Data.IsolationLevel? Isolation, NpgsqlParameter[] Parameters);
}

public sealed class PostgresTheoryAttribute : TheoryAttribute
{
    public PostgresTheoryAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(AdsApi.ConnectionSetting)))
            Skip = $"Set {AdsApi.ConnectionSetting} to run PostgreSQL integration tests.";
    }
}
