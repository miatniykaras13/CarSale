using System.Text.Json.Nodes;

namespace AdService.Application.Commands.MergePatchAd;

public record MergePatchAdCommand(Guid AdId, JsonObject Patch, Guid UserId) : ICommand<UnitResult<List<Error>>>;