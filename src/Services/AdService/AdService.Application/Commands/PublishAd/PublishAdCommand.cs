using AdService.Application.Commands.CreateAd;

namespace AdService.Application.Commands.PublishAd;

public record PublishAdCommand(Guid AdId, Guid ModeratorId) : ICommand<Result<PublishAdResponse, List<Error>>>;