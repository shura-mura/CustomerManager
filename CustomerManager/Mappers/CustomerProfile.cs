using AutoMapper;
using CustomerManager.BusinessLogic.Data.Entities;

namespace CustomerManager.Api.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile() => MapToCustomerDto();

        private void MapToCustomerDto() => CreateMap<Customer, CustomerDto>();
    }
}