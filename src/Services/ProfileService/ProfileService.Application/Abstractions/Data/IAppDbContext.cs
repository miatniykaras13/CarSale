using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Aggregates;

namespace ProfileService.Application.Abstractions.Data;

public interface IAppDbContext
{
    DbSet<UserProfile> UserProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
