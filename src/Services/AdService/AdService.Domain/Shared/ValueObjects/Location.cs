﻿using CSharpFunctionalExtensions;

namespace AdService.Domain.Shared.ValueObjects;

public record Location
{
    public string Region { get; private set; } = null!;
    public string City { get; private set; } = null!;

    protected Location()
    {
    }

    private Location(string region, string city)
    {
        Region = region;
        City = city;
    }

    public static Result<Location, Error> Of(string region, string city)
    {
        return Result.Success<Location, Error>(new Location(region, city));
    }
}