using System;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Api
{
    public record NewCustomerDto(string FirstName, string LastName, DateTime? BirthDate);
    public record CustomerDto(int Id, string FirstName, string LastName, DateTime? BirthDate);
}