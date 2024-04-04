using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.AppEntry.Commands;

namespace ViaEventAssociation.Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services, Assembly assembly)
    {
        var commandHandlerType = typeof(ICommandHandler<>);
        
        // Scan the assembly for classes that implement ICommandHandler<>
        var types = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerType))
            .ToList();

        foreach (var type in types)
        {
            var interfaceType = type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerType);
            services.AddScoped(interfaceType, type);
        }

        return services;
    }
}