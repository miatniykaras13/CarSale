using AutoCatalog.Application.Abstractions;
using AutoCatalog.Domain.Specs;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace AutoCatalog.Infrastructure.Repositories;

public class BrandsRepository(AppDbContext context) : IBrandsRepository
{
    public async Task<Result<Brand, Error>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var brand = await context.Brands.FindAsync(id, cancellationToken);
        if (brand == null)
        {
            return Result.Failure<Brand, Error>(Error.NotFound(
                nameof(Brand).ToLower(),
                $"Brand with id {id} not found."));
        }

        return Result.Success<Brand, Error>(brand);
    }

    public async Task<Result<int, Error>> AddAsync(Brand brand, CancellationToken cancellationToken)
    {
        await context.Brands.AddAsync(brand, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(brand.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Brand brand, CancellationToken cancellationToken)
    {
        context.Brands.Update(brand);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(brand.Id);
    }

    public async Task<Result<int, Error>> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var brandResult = await GetByIdAsync(id, cancellationToken);
        if (brandResult.IsFailure)
        {
            return Result.Failure<int, Error>(brandResult.Error);
        }

        var brand = brandResult.Value;
        context.Brands.Remove(brand);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(id);
    }
}