using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    public record CreateCustomerCmd(string FirstName, string LastName, DateTime? BirthDate) : IRequest<Customer>
    {
        public record Handler(IDataStorage DataStorage) : IRequestHandler<CreateCustomerCmd, Customer>
        {
            public async Task<Customer> Handle(CreateCustomerCmd request, CancellationToken cancellationToken)
            {
                var entity = await DataStorage.AddAsync(new Customer { FirstName = request.FirstName, LastName = request.LastName, BirthDate = request.BirthDate }, cancellationToken).ConfigureAwait(false);
                await DataStorage.SaveAsync(cancellationToken).ConfigureAwait(false);
                return entity;
            }
        }
    }
}