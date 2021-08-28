using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Commands
{
    public record UpdateCustomerCmd(int Id, string FirstName, string LastName, DateTime? BirthDate) : IRequest<Customer>
    {
        public record Handler(IDataStorage DataStorage) : IRequestHandler<UpdateCustomerCmd, Customer>
        {
            public async Task<Customer> Handle(UpdateCustomerCmd request, CancellationToken cancellationToken)
            {
                var entity = await DataStorage.FindAsync<Customer>(request.Id, cancellationToken).ConfigureAwait(false);
                if (entity == null)
                    return null;

                entity.FirstName = request.FirstName;
                entity.LastName = request.LastName;
                entity.BirthDate = request.BirthDate;

                await DataStorage.SaveAsync(cancellationToken).ConfigureAwait(false);

                return entity;
            }
        }
    }
}