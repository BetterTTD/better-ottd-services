using DataAccess.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DataAccessModule
{
    public static IServiceCollection AddDataAccessModule(this IServiceCollection services) => services
        .AddScoped(typeof(IGenericRepository<,>), typeof(GenericGuidRepository<,>));
}