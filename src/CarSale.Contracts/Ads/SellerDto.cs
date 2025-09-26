using CarSale.Contracts.Shared;

namespace CarSale.Contracts.Ads;

public record SellerDto(string FirstName, string LastName, PhoneNumberDto PhoneNumber);