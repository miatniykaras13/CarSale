﻿using AdService.Contracts.Ads.Default;
using BuildingBlocks.Application.Paging;

namespace AdService.Application.Queries.GetAllCarOptions;

public record GetAllCarOptionsQuery(
    CarOptionFilter Filter,
    PageParameters PageParameters) : IQuery<Result<IEnumerable<CarOptionDto>, List<Error>>>;

