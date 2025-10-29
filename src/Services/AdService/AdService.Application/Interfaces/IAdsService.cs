using CSharpFunctionalExtensions;

namespace AdService.Application.Interfaces;

public interface IAdsService
{
    Task<Result<Guid>> Create(
        CreateAdDto adDto,
        CancellationToken cancellationToken);

    Task Delete(
        Guid id,
        CancellationToken cancellationToken);

    Task Get(
        Guid id,
        CancellationToken cancellationToken);
}