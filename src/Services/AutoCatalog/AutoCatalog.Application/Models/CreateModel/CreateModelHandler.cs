using AutoCatalog.Application.Abstractions;
using AutoCatalog.Application.Cars.CreateCar;
using AutoCatalog.Domain.Enums;
using AutoCatalog.Domain.Specs;
using BuildingBlocks.CQRS;

namespace AutoCatalog.Application.Models.CreateModel;

public record CreateModelCommand(int BrandId, string Name) : ICommand<Result<int, List<Error>>>;

internal class CreateModelCommandHandler(
    IModelsRepository modelsRepository,
    IBrandsRepository brandsRepository)
    : ICommandHandler<CreateModelCommand, Result<int, List<Error>>>
{
    public async Task<Result<int, List<Error>>> Handle(CreateModelCommand command, CancellationToken cancellationToken)
    {
        var brandResult = await brandsRepository.GetByIdAsync(command.BrandId, cancellationToken);
        if (brandResult.IsFailure)
            return Result.Failure<int, List<Error>>([brandResult.Error]);
        var model = new Model() { BrandId = command.BrandId, Name = command.Name, };

        await modelsRepository.AddAsync(model, cancellationToken);

        return Result.Success<int, List<Error>>(model.Id);
    }
}