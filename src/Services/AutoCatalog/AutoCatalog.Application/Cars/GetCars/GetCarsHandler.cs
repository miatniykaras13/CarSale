using AutoCatalog.Application.Abstractions.FileStorage;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Cars.Dtos;
using BuildingBlocks.Application.Paging;
using BuildingBlocks.Application.Sorting;

namespace AutoCatalog.Application.Cars.GetCars;

public record GetCarsQuery(CarFilter Filter, SortParameters SortParameters, PageParameters PageParameters)
    : IQuery<Result<List<CarDto>, List<Error>>>;

public class GetCarsQueryHandler(
    IFileStorage fileStorage,
    ICarsRepository carsRepository) : IQueryHandler<GetCarsQuery, Result<List<CarDto>, List<Error>>>
{
    public async Task<Result<List<CarDto>, List<Error>>> Handle(GetCarsQuery query, CancellationToken cancellationToken)
    {
        var carResult = await carsRepository.GetAllAsync(
            query.Filter,
            query.SortParameters,
            query.PageParameters,
            cancellationToken);


        if (carResult.IsFailure)
            return Result.Failure<List<CarDto>, List<Error>>([carResult.Error]);

        var cars = carResult.Value;

        var imageTasks = cars.Select(c =>
                c.PhotoId is not null
                    ? fileStorage.GetDownloadLinkAsync(c.PhotoId.Value, 300, cancellationToken)
                    : null)
            .ToList();

        await Task.WhenAll(imageTasks.Where(t => t is not null)!);

        var carDtos = new List<CarDto>(cars.Count);

        for (int i = 0; i < cars.Count; i++)
        {
            var c = cars[i];
            carDtos.Add(new CarDto(
                c.Id,
                new BrandDto(c.Brand.Id, c.Brand.Name),
                new ModelDto(c.Model.Id, c.Model.Name),
                new GenerationDto(
                    c.Generation.Id,
                    c.Generation.Name,
                    c.Generation.YearFrom,
                    c.Generation.YearTo ?? DateTime.UtcNow.Year),
                new EngineDto(
                    c.Engine.Id,
                    c.Engine.Name,
                    new FuelTypeDto(c.Engine.FuelType.Id, c.Engine.FuelType.Name),
                    c.Engine.Volume,
                    c.Engine.HorsePower,
                    c.Engine.TorqueNm),
                new TransmissionTypeDto(c.TransmissionType.Id, c.TransmissionType.Name),
                new AutoDriveTypeDto(c.DriveType.Id, c.DriveType.Name),
                imageTasks[i] is not null ? await imageTasks[i]! : null,
                c.Consumption,
                c.Acceleration,
                c.FuelTankCapacity,
                new DimensionsDto(c.Dimensions.Width, c.Dimensions.Height, c.Dimensions.Length)));
        }


        return Result.Success<List<CarDto>, List<Error>>(carDtos);
    }
}