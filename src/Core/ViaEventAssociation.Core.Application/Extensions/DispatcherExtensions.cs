using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.AppEntry.Dispatcher;

namespace ViaEventAssociation.Core.Application.Extensions;

public static class DispatcherExtensions
{
    
    public static void RegisterDispatcher(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher>();
    }
}