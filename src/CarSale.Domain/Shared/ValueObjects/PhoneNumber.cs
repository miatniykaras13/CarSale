using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace CarSale.Domain.Shared.ValueObjects
{
    public record PhoneNumber(CountryCode CountryCode, string Number)
    {
        //public static Result<PhoneNumber> Of(string number)
        //{
            //var regex = new Regex(@"^\u002b\d{12}$");
            //var matches = regex.Matches(number);

            //if (matches.Count != 1)
            //{
            //    return Result.Failure<PhoneNumber>("Invalid number");
            //
        //}

    }
}
