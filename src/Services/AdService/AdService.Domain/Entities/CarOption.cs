using AdService.Domain.Enums;
using CSharpFunctionalExtensions;

namespace AdService.Domain.Entities;

public class CarOption : BuildingBlocks.DDD.Abstractions.Entity<int>
{
    public const int MAX_NAME_LENGTH = 100;
    public const int MIN_NAME_LENGTH = 3;
    public const int MAX_TECHNAME_LENGTH = 20;
    public const int MIN_TECHNAME_LENGTH = 3;

    public OptionType OptionType { get; private set; }

    public string Name { get; private set; }

    public string TechnicalName { get; private set; }

    private CarOption(OptionType optionType, string name, string technicalName)
    {
        OptionType = optionType;
        Name = name;
        TechnicalName = technicalName;
    }

    protected CarOption()
    {
    }


    public static Result<CarOption, Error> Create(OptionType optionType, string name, string technicalName)
    {
        if (name.Length is < MIN_NAME_LENGTH or > MAX_NAME_LENGTH)
        {
            return Result.Failure<CarOption, Error>(Error.Domain(
                "car_option.name.is.conflict",
                $"Name must be between {MIN_NAME_LENGTH} and {MAX_NAME_LENGTH} characters."));
        }

        if (technicalName.Length is < MIN_TECHNAME_LENGTH or > MAX_TECHNAME_LENGTH)
        {
            return Result.Failure<CarOption, Error>(Error.Domain(
                "car_option.technical_name.is.conflict",
                $"Technical name must be between {MIN_TECHNAME_LENGTH} and {MAX_TECHNAME_LENGTH} characters."));
        }

        CarOption conf = new(optionType, name, technicalName);
        return Result.Success<CarOption, Error>(conf);
    }
}