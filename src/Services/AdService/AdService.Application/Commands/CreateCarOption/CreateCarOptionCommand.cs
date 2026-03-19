using AdService.Domain.Enums;

namespace AdService.Application.Commands.CreateCarOption;

public record CreateCarOptionCommand(OptionType OptionType, string Name, string TechnicalName)
    : ICommand<Result<int, List<Error>>>;
