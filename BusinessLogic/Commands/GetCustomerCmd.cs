using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    public record GetCustomerCmd(int Id) : IRequest<Customer>
    {
        public record Handler(IReadOnlyDataStorage DataStorage) : IRequestHandler<GetCustomerCmd, Customer>
        {
            public Task<Customer> Handle(GetCustomerCmd request, CancellationToken cancellationToken) => DataStorage.FindAsync<Customer>(request.Id, cancellationToken).AsTask();
        }
    }
}