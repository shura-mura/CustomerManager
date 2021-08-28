using CustomerManager.BusinessLogic.Data;
using CustomerManager.Data.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CustomerManager.BusinessLogic
{
    public abstract class BaseTests
    {
        private IDataStorageFactory _dataStorageFactory;
        private IServiceProvider _serviceProvider;

        [SetUp]
        public void SetUp()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDataStorage();

            _serviceProvider = services.BuildServiceProvider();
            _dataStorageFactory = _serviceProvider.GetRequiredService<IDataStorageFactory>();
            _dataStorageFactory.CreateStorage();
        }

        [TearDown]
        public void TearDown() => _dataStorageFactory?.Dispose();

        protected async Task<TResult> Execute<TResult>(Func<IServiceProvider, Task<TResult>> action)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                IDataStorage dataStorage = scope.ServiceProvider.GetRequiredService<IDataStorage>();
                return await action(scope.ServiceProvider);
            }
        }

        protected Task Execute(Func<IServiceProvider, Task> action) =>
            Execute(async sp =>
                            {
                                await action(sp);
                                return 0;
                            });
    }
}