using AutoCatalog.Web.Contracts;
using AutoCatalog.Web.Domain.Cars;
using AutoCatalog.Web.Domain.Enums;
using AutoCatalog.Web.Domain.Specs;
using BuildingBlocks.CQRS;

namespace AutoCatalog.Web.Features.Cars.CreateCar;

public record CreateCarCommand(
    int BrandId,
    int ModelId,
    int GenerationId,
    int EngineId,
    TransmissionType TransmissionType,
    AutoDriveType AutoDriveType,
    int YearFrom,
    int YearTo,
    Guid PhotoId,
    int Consumption,
    float Acceleration,
    int FuelTankCapacity,
    DimensionsDto DimensionsDto) : ICommand<CreateCarResult>;

public record CreateCarResult(Guid Id);

internal class CreateCarCommandHandler : ICommandHandler<CreateCarCommand, CreateCarResult>
{
    public async Task<CreateCarResult> Handle(CreateCarCommand command, CancellationToken cancellationToken)
    {
        var car = new Car
        {
            Id = Guid.CreateVersion7(),
            BrandId = command.BrandId,
            ModelId = command.ModelId,
            GenerationId = command.GenerationId,
            EngineId = command.EngineId,
            TransmissionType = command.TransmissionType,
            AutoDriveType = command.AutoDriveType,
            YearFrom = command.YearFrom,
            YearTo = command.YearTo,
            PhotoId = command.PhotoId,
            Consumption = command.Consumption,
            Acceleration = command.Acceleration,
            FuelTankCapacity = command.FuelTankCapacity,
            Dimensions = new Dimensions(
                command.DimensionsDto.Width,
                command.DimensionsDto.Height,
                command.DimensionsDto.Length),
        };


        return new CreateCarResult(car.Id);
    }
}