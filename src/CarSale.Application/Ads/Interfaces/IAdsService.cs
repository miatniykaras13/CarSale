using CarSale.Contracts.Ads;
using CSharpFunctionalExtensions;

namespace CarSale.Application.Ads;

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