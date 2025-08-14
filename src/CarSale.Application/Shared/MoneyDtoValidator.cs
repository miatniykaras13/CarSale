using CarSale.Contracts.Shared;
using FluentValidation;

namespace CarSale.Application.Shared;

public class MoneyDtoValidator : AbstractValidator<MoneyDto>
{
    public MoneyDtoValidator()
    {
        RuleFor(x => x.Amount).
            NotNull().WithMessage("Money's amount must not be null..").
            LessThan(int.MaxValue).WithMessage("Money's amount is too large");

        RuleFor(x => x.CurrencyDto).SetValidator(new CurrencyDtoValidator());
    }
}