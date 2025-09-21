using CarSale.Contracts.Ads;
using CarSale.Domain.Ads.Enums;
using CarSale.Domain.Ads.ValueObjects;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CarSale.Application.Shared.Factories
{
    public class CarConfigurationFactory
    {
        public static Result<CarConfiguration> FromDto(CarConfigurationDto configDto)
        {
            var interiorType = Enum.Parse<InteriorType>(configDto.InteriorType);
            var opticsType = Enum.Parse<OpticsType>(configDto.OpticsType);
            var safetyOptionsResult = ParseFlags<SafetyOptions>(configDto.SafetyOptions);
            var comfortOptionsResult = ParseFlags<ComfortOptions>(configDto.ComfortOptions);
            var assistanceOptionsResult = ParseFlags<AssistanceOptions>(configDto.AssistanceOptions);

            var combinedResult = Result.Combine(safetyOptionsResult, comfortOptionsResult, assistanceOptionsResult);

            if (combinedResult.IsFailure)
                return Result.Failure<CarConfiguration>(combinedResult.Error);

            var configResult = CarConfiguration.Of(
                interiorType,
                safetyOptionsResult.Value,
                comfortOptionsResult.Value,
                assistanceOptionsResult.Value,
                opticsType);

            if (configResult.IsFailure)
                return Result.Failure<CarConfiguration>(configResult.Error);

            return Result.Success(configResult.Value);
        }

        private static Result<TEnum> ParseFlags<TEnum>(IEnumerable<string> values)
            where TEnum : struct, Enum
        {
            IEnumerable<string> enumerable = values as string[] ?? values.ToArray();
            var invalid = enumerable.Where(v => !Enum.TryParse<TEnum>(v, true, out _)).ToList();

            if (invalid.Count != 0)
                return Result.Failure<TEnum>($"Unsupported values: {string.Join(", ", invalid)}");

            var combined = enumerable
                .Select(v => Enum.Parse<TEnum>(v, true))
                .Aggregate(default(TEnum), (acc, next) => (TEnum)(object)((int)(object)acc | (int)(object)next));

            return Result.Success(combined);
        }
    }
}
