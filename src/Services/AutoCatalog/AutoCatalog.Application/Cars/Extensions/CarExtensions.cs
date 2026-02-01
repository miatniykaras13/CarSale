using System.Linq.Expressions;
using AutoCatalog.Domain.Transport.Cars;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Cars.Extensions;

public static class CarExtensions
{
    public static IQueryable<Car> Filter(this IQueryable<Car> query, CarFilter filter)
    {
        // brand
        if (filter.BrandId != null)
            query = query.Where(c => c.BrandId == filter.BrandId);

        if (!string.IsNullOrEmpty(filter.BrandName))
            query = query.Where(c => c.Brand.Name.Equals(filter.BrandName, StringComparison.OrdinalIgnoreCase));

        // model
        if (filter.ModelId != null)
            query = query.Where(c => c.ModelId == filter.ModelId);

        if (!string.IsNullOrEmpty(filter.ModelName))
            query = query.Where(c => c.Model.Name.Equals(filter.ModelName, StringComparison.OrdinalIgnoreCase));

        // generation
        if (filter.GenerationId != null)
            query = query.Where(c => c.GenerationId == filter.GenerationId);

        if (!string.IsNullOrEmpty(filter.GenerationName))
        {
            query = query.Where(c =>
                c.Generation.Name.Equals(filter.GenerationName, StringComparison.OrdinalIgnoreCase));
        }

        // engine
        if (filter.EngineId != null)
            query = query.Where(c => c.EngineId == filter.EngineId);

        if (!string.IsNullOrEmpty(filter.EngineName))
            query = query.Where(c => c.Engine.Name.Equals(filter.EngineName, StringComparison.OrdinalIgnoreCase));

        // transmission type
        if (filter.TransmissionTypeId != null)
            query = query.Where(c => c.TransmissionTypeId == filter.TransmissionTypeId);

        if (!string.IsNullOrEmpty(filter.TransmissionType))
        {
            query = query.Where(c =>
                c.TransmissionType.Name.Equals(filter.TransmissionType, StringComparison.OrdinalIgnoreCase));
        }

        // drive type
        if (filter.DriveTypeId != null)
            query = query.Where(c => c.DriveTypeId == filter.DriveTypeId);

        if (!string.IsNullOrEmpty(filter.DriveType))
            query = query.Where(c => c.DriveType.Name.Equals(filter.DriveType, StringComparison.OrdinalIgnoreCase));

        // body type
        if (filter.BodyTypeId != null)
            query = query.Where(c => c.BodyTypeId == filter.BodyTypeId);

        if (!string.IsNullOrEmpty(filter.BodyType))
            query = query.Where(c => c.BodyType.Name.Equals(filter.BodyType, StringComparison.OrdinalIgnoreCase));

        if (filter.Acceleration != null)
            query = query.Where(c => Equals(c.Acceleration, filter.Acceleration));

        if (filter.Consumption != null)
            query = query.Where(c => Equals(c.Consumption, filter.Consumption));

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
            nameof(Car.DriveType) => x => x.DriveType,
            nameof(Car.FuelTankCapacity) => x => x.FuelTankCapacity,
            nameof(Car.TransmissionType) => x => x.TransmissionType,
            nameof(Car.Consumption) => x => x.Consumption,
            nameof(Car.BodyType) => x => x.BodyType,
            _ => x => x.Id
        };
    }
}