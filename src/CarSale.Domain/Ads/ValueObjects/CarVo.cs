using CarSale.Domain.Shared.ValueObjects;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Ads.ValueObjects;

public record CarVo(
    string Brand,
    string Model,
    int Year,
    string Generation,
    string Vin,
    int Mileage,
    string Color) // car value object
{
    private const int REQUIRED_VIN_LENGTH = 17;
    
    public static int RequiredVinLength => REQUIRED_VIN_LENGTH;
    
    public static Result<CarVo> Of(
        string brand,
        string model,
        int year,
        string generation,
        string vin,
        int mileage,
        string color)
    {
        if (brand.Length <= 0)
        {
            return Result.Failure<CarVo>("Brand's length must be greater than zero.");
        }

        if (model.Length <= 0)
        {
            return Result.Failure<CarVo>("Model's length must be greater than zero.");
        }

        if (year <= 1900)
        {
            return Result.Failure<CarVo>("Invalid year");
        }

        if (vin.Length != REQUIRED_VIN_LENGTH)
        {
            return Result.Failure<CarVo>("Invalid vin length");
        }

        if (mileage < 0)
        {
            return Result.Failure<CarVo>("Invalid mileage");
        }

        if (color.Length <= 0)
        {
            return Result.Failure<CarVo>("Color's length must be greater than zero.");
        }

        var carVo = new CarVo(brand, model, year, generation, vin, mileage, color);
        return Result.Success(carVo);
    }
}