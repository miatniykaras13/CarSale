using System.Linq.Expressions;
using AutoCatalog.Application.Cars;
using AutoCatalog.Domain.Specs;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Cars.Extensions;

public static class CarExtensions
{
    private static readonly Dictionary<string, LambdaExpression> _map = new(StringComparer.OrdinalIgnoreCase) { };

    public static IQueryable<Car> Filter(this IQueryable<Car> query, CarFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.BrandName))
            query = query.Where(c => c.Brand.Name.ToLower().Equals(filter.BrandName.ToLower()));

        if (!string.IsNullOrEmpty(filter.ModelName))
            query = query.Where(c => c.Model.Name.ToLower().Equals(filter.ModelName.ToLower()));

        if (!string.IsNullOrEmpty(filter.GenerationName))
            query = query.Where(c => c.Generation.Name.ToLower().Equals(filter.GenerationName.ToLower()));

        if (!string.IsNullOrEmpty(filter.EngineName))
            query = query.Where(c => c.Engine.Name.ToLower().Equals(filter.EngineName.ToLower()));

        if (filter.TransmissionType != null && filter.TransmissionType.Length != 0)
            query = query.Where(c => filter.TransmissionType.Contains(c.TransmissionType));

        if (filter.AutoDriveType != null && filter.AutoDriveType.Length != 0)
            query = query.Where(c => filter.AutoDriveType.Contains(c.AutoDriveType));

        if (filter.Year != null)
            query = query.Where(c => c.YearFrom <= filter.Year && c.YearTo >= filter.Year);

        if (filter.Acceleration != null)
            query = query.Where(c => c.Acceleration == filter.Acceleration);

        if (filter.FuelTankCapacity != null)
            query = query.Where(c => c.FuelTankCapacity == filter.FuelTankCapacity);

        return query;
    }

    public static IQueryable<Car> Page(this IQueryable<Car> query, PageParameters pageParameters) =>
        query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize).Take(pageParameters.PageSize);


    public static IQueryable<Car> Sort(this IQueryable<Car> query, SortParameters sortParameters) =>
        sortParameters.Direction == SortDirection.ASCENDING
            ? query.OrderBy(GetKeySelector(sortParameters.OrderBy))
            : query.OrderByDescending(GetKeySelector(sortParameters.OrderBy));


    private static Expression<Func<Car, object>> GetKeySelector(string? orderBy)
    {
        return orderBy switch
        {
            nameof(Car.Acceleration) => x => x.Acceleration,
            nameof(Car.AutoDriveType) => x => x.AutoDriveType,
            nameof(Car.FuelTankCapacity) => x => x.FuelTankCapacity,
            nameof(Car.TransmissionType) => x => x.TransmissionType,
            nameof(Car.YearFrom) => x => x.YearFrom,
            nameof(Car.YearTo) => x => x.YearTo,
            _ => x => x.Id
        };
    }
}