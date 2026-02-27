﻿using AdService.Application.Abstractions.Data;
using AdService.Contracts.Ads.Default;

namespace AdService.Application.Queries.GetAllCarOptions;

public class GetAllCarOptionsQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetAllCarOptionsQuery, Result<IEnumerable<CarOptionDto>, List<Error>>>
{
    public async Task<Result<IEnumerable<CarOptionDto>, List<Error>>> Handle(
        GetAllCarOptionsQuery query,
        CancellationToken ct)
    {
        var carOptions = await dbContext.CarOptions
            .AsNoTracking()
            .Filter(query.Filter)
            .OrderBy(o => o.Id)
            .Page(query.PageParameters)
            .Select(o => new CarOptionDto(o.Id, o.OptionType.ToString(), o.Name, o.TechnicalName))
            .ToListAsync(ct);

        return Result.Success<IEnumerable<CarOptionDto>, List<Error>>(carOptions);
    }
}
