using System.Text.Json.Nodes;
using AdService.Contracts.Ads;

namespace AdService.Application.Commands.MergePatchAdsCar;

public record MergePatchAdsCarCommand(Guid AdId, JsonObject Patch) : ICommand<UnitResult<Error>>;