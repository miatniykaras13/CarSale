using AutoCatalog.Web.Contracts;
using AutoCatalog.Web.Domain.Enums;

namespace AutoCatalog.Web.Features.Cars.CreateCar;

public record CreateCarRequest(
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
    DimensionsDto DimensionsDto);

public record CreateCarResponse(Guid Id);

public class CreateCarEndpoint
{
    
}