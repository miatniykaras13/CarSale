using AutoCatalog.Application.Abstractions.FileStorage;
using AutoCatalog.Application.Abstractions.Repositories;
using AutoCatalog.Application.Cars.Dtos;
using AutoCatalog.Domain.Transport.Cars;

namespace AutoCatalog.Application.Cars.GetCarById;

public record GetCarByIdQuery(Guid Id) : IQuery<Result<CarDto, List<Error>>>;

public class GetCarByIdQueryHandler(ICarsRepository carsRepository, IFileStorage fileStorage)
    : IQueryHandler<GetCarByIdQuery, Result<CarDto, List<Error>>>
{
    public async Task<Result<CarDto, List<Error>>> Handle(GetCarByIdQuery query, CancellationToken cancellationToken)
    {
        var carResult = await carsRepository.GetByIdAsync(query.Id, cancellationToken);
        if (carResult.IsFailure)
            return Result.Failure<CarDto, List<Error>>([carResult.Error]);

        var car = carResult.Value;

        var imageUrl = car.PhotoId is not null
            ? await fileStorage.GetDownloadLinkAsync(car.PhotoId.Value, 300, cancellationToken)
            : null;

        var carDto = new CarDto(
            car.Id,
            new BrandDto(car.Brand.Id, car.Brand.Name),
            new ModelDto(car.Model.Id, car.Model.Name),
            new GenerationDto(
                car.Generation.Id,
                car.Generation.Name,
                car.Generation.YearFrom,
                car.Generation.YearTo ?? DateTime.UtcNow.Year),
            new EngineDto(
                car.Engine.Id,
                car.Engine.Name,
                new FuelTypeDto(car.Engine.FuelType.Id, car.Engine.FuelType.Name),
                car.Engine.Volume,
                car.Engine.HorsePower,
                car.Engine.TorqueNm),
            new TransmissionTypeDto(car.TransmissionType.Id, car.TransmissionType.Name),
            new AutoDriveTypeDto(car.DriveType.Id, car.DriveType.Name),
            new BodyTypeDto(car.BodyType.Id, car.BodyType.Name),
            imageUrl,
            car.Consumption,
            car.Acceleration,
            car.FuelTankCapacity,
            new DimensionsDto(car.Dimensions.Width, car.Dimensions.Height, car.Dimensions.Length));

        return Result.Success<CarDto, List<Error>>(carDto);
    }
}