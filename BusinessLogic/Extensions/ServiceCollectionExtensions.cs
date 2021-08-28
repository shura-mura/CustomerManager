using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerManager.BusinessLogic.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services) => services.AddMediatR(typeof(ServiceCollectionExtensions));
    }
}