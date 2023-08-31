using EventSourcing.Application.Common.Interfaces;
using EventSourcing.Domain.Entities;
using EventSourcing.Domain.Entities.Common;
using EventSourcing.Infrastructure.Persistence;
using EventStore.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EventSourcing.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddEventStore(this IServiceCollection services, IConfiguration configuration)
    {
        // Event store database connection
        var settings = EventStoreClientSettings
            .Create("esdb://127.0.0.1:2113?tls=false&keepAliveTimeout=10000&keepAliveInterval=10000");

        var client = new EventStoreClient(settings);
        services.AddSingleton(client);

        // Register DbContext for SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                });
        });

        services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();
        services.AddEventsRepository<CatalogItem, Guid>();

        return services;

    }


    private static IServiceCollection AddEventsRepository<TA, TK>(this IServiceCollection services)
        where TA : class, IAggregateRoot<TK>
    {
        return services.AddSingleton(typeof(IAggregateRepository<TA, TK>), typeof(AggregateRepository<TA, TK>));
    }
}