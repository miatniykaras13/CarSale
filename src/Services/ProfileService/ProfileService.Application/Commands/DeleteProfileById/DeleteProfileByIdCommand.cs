using BuildingBlocks.CQRS;
using BuildingBlocks.Errors;
using CSharpFunctionalExtensions;

namespace ProfileService.Application.Commands.DeleteProfileById;

public record DeleteProfileByIdCommand(Guid ProfileId) : ICommand<UnitResult<List<Error>>>;
