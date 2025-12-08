using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Domain.ValueObjects;

public record Address
{
    public string Region { get; private set; } = null!;
    public string City { get; private set; } = null!;

    protected Address()
    {
    }

    private Address(string region, string city)
    {
        Region = region;
        City = city;
    }

    public static Result<Address, Error> Of(string region, string city)
    {
        return Result.Success<Address, Error>(new Address(region, city));
    }
}