namespace AdService.Application.Commands.DeleteCarOption;

public record DeleteCarOptionCommand(int CarOptionId) : ICommand<UnitResult<List<Error>>>;

