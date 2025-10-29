using CSharpFunctionalExtensions;

namespace AdService.Application.Shared.Factories;

public class CarVoFactory
{
    public static Result<CarVo> FromDto(CarVoDto carDto)
    {
        Result<CarVo> carResult = CarVo.Of(
            carDto.Brand,
            carDto.Model,
            carDto.Year,
            carDto.Generation,
            carDto.Vin,
            carDto.Mileage,
            carDto.Color);

        if (carResult.IsFailure)
        {
            return Result.Failure<CarVo>(carResult.Error);
        }

        return Result.Success(carResult.Value);
    }
}