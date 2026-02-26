using AdService.Domain.Aggregates;

namespace AdService.Application.Queries;

public static class AdExtensions
{
    public static IQueryable<Ad> Filter(this IQueryable<Ad> ads, AdFilter filter)
    {
        // brand
        if (filter.BrandId != null)
            ads = ads.Where(a => a.Car != null && a.Car.Brand != null && a.Car.Brand.Id == filter.BrandId);

        if (!string.IsNullOrEmpty(filter.BrandName))
        {
            var brandName = filter.BrandName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.Brand != null && a.Car.Brand.Name.ToLower().Equals(brandName));
        }

        // model
        if (filter.ModelId != null)
            ads = ads.Where(a => a.Car != null && a.Car.Model != null && a.Car.Model.Id == filter.ModelId);

        if (!string.IsNullOrEmpty(filter.ModelName))
        {
            var modelName = filter.ModelName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.Model != null && a.Car.Model.Name.ToLower().Equals(modelName));
        }

        // generation
        if (filter.GenerationId != null)
        {
            ads = ads.Where(a =>
                a.Car != null && a.Car.Generation != null && a.Car.Generation.Id == filter.GenerationId);
        }

        if (!string.IsNullOrEmpty(filter.GenerationName))
        {
            var generationName = filter.GenerationName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.Generation != null &&
                                 a.Car.Generation.Name.ToLower().Equals(generationName));
        }

        // drive type
        if (filter.DriveTypeId != null)
            ads = ads.Where(a => a.Car != null && a.Car.DriveType != null && a.Car.DriveType.Id == filter.DriveTypeId);

        if (!string.IsNullOrEmpty(filter.DriveTypeName))
        {
            var driveTypeName = filter.DriveTypeName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.DriveType != null &&
                                 a.Car.DriveType.Name.ToLower().Equals(driveTypeName));
        }

        // body type
        if (filter.BodyTypeId != null)
            ads = ads.Where(a => a.Car != null && a.Car.BodyType != null && a.Car.BodyType.Id == filter.BodyTypeId);

        if (!string.IsNullOrEmpty(filter.BodyTypeName))
        {
            var bodyTypeName = filter.BodyTypeName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.BodyType != null &&
                                 a.Car.BodyType.Name.ToLower().Equals(bodyTypeName));
        }

        // fuel type
        if (filter.FuelTypeId != null)
        {
            ads = ads.Where(a => a.Car != null && a.Car.Engine != null &&
                                 a.Car.Engine.FuelType.Id == filter.FuelTypeId);
        }

        if (!string.IsNullOrEmpty(filter.FuelTypeName))
        {
            var fuelTypeName = filter.FuelTypeName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.Engine != null &&
                                 a.Car.Engine.FuelType.Name.ToLower().Equals(fuelTypeName));
        }

        // transmission type
        if (filter.TransmissionTypeId != null)
        {
            ads = ads.Where(a => a.Car != null && a.Car.TransmissionType != null &&
                                 a.Car.TransmissionType.Id == filter.TransmissionTypeId);
        }

        if (!string.IsNullOrEmpty(filter.TransmissionTypeName))
        {
            var transmissionTypeName = filter.TransmissionTypeName.ToLower();
            ads = ads.Where(a => a.Car != null && a.Car.TransmissionType != null &&
                                 a.Car.TransmissionType.Name.ToLower().Equals(transmissionTypeName));
        }

        // price
        if (filter.PriceFrom != null)
            ads = ads.Where(a => a.Price != null && a.Price.Amount >= filter.PriceFrom);

        if (filter.PriceTo != null)
            ads = ads.Where(a => a.Price != null && a.Price.Amount <= filter.PriceTo);

        // mileage
        if (filter.MileageFrom != null)
            ads = ads.Where(a => a.Car != null && a.Car.Mileage != null && a.Car.Mileage >= filter.MileageFrom);

        if (filter.MileageTo != null)
            ads = ads.Where(a => a.Car != null && a.Car.Mileage != null && a.Car.Mileage <= filter.MileageTo);

        // year
        if (filter.YearFrom != null)
            ads = ads.Where(a => a.Car != null && a.Car.Year != null && a.Car.Year >= filter.YearFrom);

        if (filter.YearTo != null)
            ads = ads.Where(a => a.Car != null && a.Car.Year != null && a.Car.Year <= filter.YearTo);

        return ads;
    }
}