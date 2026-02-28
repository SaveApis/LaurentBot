using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;

namespace Utils.Mediator.Application.DI;

public class MediatorModule(Assembly softwareAssembly) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddMediatR(configuration =>
            {
                configuration.RegisterGenericHandlers = true;
                configuration.Lifetime = ServiceLifetime.Scoped;
                configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly(), softwareAssembly);
            }
        );

        builder.Populate(collection);
    }
}
