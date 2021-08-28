using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    public record FindCustomerCmd(string FirstName, string LastName) : IRequest<IEnumerable<Customer>>
    {
        public record Handler(IReadOnlyDataStorage DataStorage) : IRequestHandler<FindCustomerCmd, IEnumerable<Customer>>
        {
            public Task<IEnumerable<Customer>> Handle(FindCustomerCmd request, CancellationToken cancellationToken)
            {
                IEnumerable<Customer> entities = DataStorage.Customers.Where(x => x.FirstName.Contains(request.FirstName) || x.LastName.Contains(request.LastName)).ToArray();
                return Task.FromResult(entities);
            }
        }
    }
}