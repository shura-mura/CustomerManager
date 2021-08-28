using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    public record DeleteCustomerCmd(int Id) : IRequest<Customer>
    {
        public record Handler(IDataStorage DataStorage) : IRequestHandler<DeleteCustomerCmd, Customer>
        {
            public async Task<Customer> Handle(DeleteCustomerCmd request, CancellationToken cancellationToken)
            {
                var entity = await DataStorage.FindAsync<Customer>(request.Id, cancellationToken).ConfigureAwait(false);
                if (entity == null)
                    return null;

                DataStorage.Remove(entity);
                await DataStorage.SaveAsync(cancellationToken).ConfigureAwait(false);
                return entity;
            }
        }
    }
}