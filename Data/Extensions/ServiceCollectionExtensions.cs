using CustomerManager.BusinessLogic.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManager.Data.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataStorage(this IServiceCollection services) =>
            services
                .AddSingleton<IDataStorageFactory, DataStorageFactory>()
                .AddScoped<IDataStorage>(_ => new DataStorage(false))
                .AddScoped<IReadOnlyDataStorage>(_ => new DataStorage(true));
    }
}