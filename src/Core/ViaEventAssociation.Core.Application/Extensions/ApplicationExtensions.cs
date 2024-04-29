using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace ViaEventAssociation.Core.Application.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<ViaEventCreateCommand>, ViaEventCreateHandler>();
    }
}