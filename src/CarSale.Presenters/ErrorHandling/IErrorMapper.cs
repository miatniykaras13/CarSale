using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace CarSale.Presenters.ErrorHandling;

public interface IErrorMapper
{
    public IActionResult ToActionResult(Result result);
}