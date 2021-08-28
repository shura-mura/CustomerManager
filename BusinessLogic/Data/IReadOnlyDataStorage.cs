using CustomerManager.BusinessLogic.Data.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Data
{
    public interface IReadOnlyDataStorage : IDisposable
    {
        IEntityCollection<Customer> Customers { get; }

        ValueTask<TEntity> FindAsync<TEntity>(object keyValue, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity;
    }
}