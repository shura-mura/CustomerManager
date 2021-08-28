using CustomerManager.BusinessLogic.Data;
using CustomerManager.BusinessLogic.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerManager.Data
{
    public sealed class DataStorage : DbContext, IDataStorage
    {
        private static readonly MethodInfo _applyGenericMethod;

        private readonly bool _isReadonly;

        static DataStorage()
        {
            _applyGenericMethod = typeof(ModelBuilder).GetMethods(BindingFlags.Instance | BindingFlags.Public).Single(
                x =>
                {
                    ParameterInfo[] parameters = x.GetParameters();
                    return x.Name == nameof(ModelBuilder.ApplyConfiguration) &&
                            parameters.Length == 1 &&
                            parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);
                });
        }

        public DataStorage(bool isReadonly) => _isReadonly = isReadonly;

        IEntityCollection<Customer> IReadOnlyDataStorage.Customers => GetCollection(Customers);

        public DbSet<Customer> Customers { get; set; }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            if (_isReadonly)
                return Task.CompletedTask;

            return SaveChangesAsync(cancellationToken);
        }

        async ValueTask<TEntity> IDataStorage.AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken)
        {
            EntityEntry<TEntity> entry = await AddAsync(entity, cancellationToken).ConfigureAwait(false);
            return entry.Entity;
        }

        TEntity IDataStorage.Remove<TEntity>(TEntity entity) => Remove(entity).Entity;

        ValueTask<TEntity> IReadOnlyDataStorage.FindAsync<TEntity>(object keyValue, CancellationToken cancellationToken) =>
            FindAsync<TEntity>(new object[] { keyValue }, cancellationToken);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!_isReadonly)
                optionsBuilder.UseLazyLoadingProxies();

            optionsBuilder.UseSqlite("Data Source=sharedmemdb;Mode=Memory;Cache=Shared");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (Type type in typeof(DataStorage).Assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract))
            {
                foreach (Type @interface in type.GetInterfaces())
                {
                    if (@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        _applyGenericMethod.MakeGenericMethod(@interface.GenericTypeArguments[0]).Invoke(
                            modelBuilder,
                            new object[] { Activator.CreateInstance(type) });
                    }
                }
            }
        }

        private IEntityCollection<TEntity> GetCollection<TEntity>(IQueryable<TEntity> set)
            where TEntity : class, IEntity => new EntityCollection<TEntity>(set, _isReadonly);
    }
}