using System.Linq.Expressions;
using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Brands;
using AutoCatalog.Application.Brands.Extensions;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Infrastructure.Repositories.Specs;

public class BrandsRepository(AppDbContext context) : IBrandsRepository
{
    public async Task<Result<Brand, Error>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var brand = await context.Brands.FindAsync([id], cancellationToken);
        if (brand == null)
        {
            return Result.Failure<Brand, Error>(Error.NotFound(
                nameof(Brand).ToLower(),
                $"Brand with id {id} not found."));
        }

        return Result.Success<Brand, Error>(brand);
    }

    public async Task<Result<List<Brand>, Error>> GetAllAsync(
        BrandFilter brandFilter,
        SortParameters sortParameters,
        PageParameters pageParameters,
        CancellationToken cancellationToken = default)
    {
        var brands = await context.Brands
            .AsNoTracking()
            .Filter(brandFilter)
            .Sort(sortParameters)
            .Page(pageParameters)
            .ToListAsync(cancellationToken);
        return Result.Success<List<Brand>, Error>(brands);
    }

    public async Task<Result<int, Error>> AddAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        await context.Brands.AddAsync(brand, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(brand.Id);
    }

    public async Task<Result<int, Error>> UpdateAsync(Brand brand, CancellationToken cancellationToken = default)
    {
        context.Brands.Update(brand);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<int, Error>(brand.Id);
    }

    public async Task<Result<Unit, Error>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var brandResult = await GetByIdAsync(id, cancellationToken);
        if (brandResult.IsFailure)
        {
            return Result.Failure<Unit, Error>(brandResult.Error);
        }

        var brand = brandResult.Value;
        context.Brands.Remove(brand);
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success<Unit, Error>(Unit.Value);
    }

    public async Task<Result<Brand, Error>> GetByName(string name, CancellationToken ct = default)
    {
        var brand = await context.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.Name == name, ct);
        if (brand == null)
        {
            return Result.Failure<Brand, Error>(Error.NotFound(
                nameof(Brand).ToLower(),
                $"Brand with name {name} not found."));
        }

        return Result.Success<Brand, Error>(brand);
    }
}