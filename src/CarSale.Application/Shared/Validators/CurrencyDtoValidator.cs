using CarSale.Contracts.Shared;
using CarSale.Domain.Shared.ValueObjects;
using FluentValidation;

namespace CarSale.Application.Shared.Validators;

public class CurrencyDtoValidator : AbstractValidator<CurrencyDto>
{
    public CurrencyDtoValidator()
    {
        List<string> currencyList = Currency.GetSupportedCurrencies();

        RuleFor(x => x.CurrencyCode).NotEmpty().WithMessage("Currency code is required.")
            .Must(c => currencyList.Contains(c)).WithMessage("Not supported currency.");
    }
}