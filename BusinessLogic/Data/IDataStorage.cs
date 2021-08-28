using CustomerManager.BusinessLogic.Data.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic.Data
{
    public interface IDataStorage : IReadOnlyDataStorage
    {
        ValueTask<TEntity> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
            where TEntity : class, IEntity;

        TEntity Remove<TEntity>(TEntity entity)
            where TEntity : class, IEntity;

        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}