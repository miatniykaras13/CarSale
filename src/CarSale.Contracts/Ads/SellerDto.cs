using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSale.Contracts.Shared;

namespace CarSale.Contracts.Ads
{
    public record SellerDto(string FirstName, string LastName, PhoneNumberDto PhoneNumber);
}
