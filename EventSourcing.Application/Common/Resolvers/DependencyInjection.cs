using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcing.Application.Common.Resolvers;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //Add MediatR to the Pipe line
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}