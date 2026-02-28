using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Utils.Mediator.Application.DI;
using Utils.Mediator.Application.Extensions;
using Utils.Serilog.Application.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule(new SerilogModule("Backend"));
            containerBuilder.RegisterModule(new MediatorModule(Assembly.GetExecutingAssembly()));
        }
    );

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunWithEventAsync().ConfigureAwait(false);
