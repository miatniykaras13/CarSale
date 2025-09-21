using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarSale.Presenters.ErrorHandling
{
    public class ErrorMapper : IErrorMapper
    {
        public IActionResult ToActionResult(Result result) => switch result.Error
        {

        };
    }
}
