using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Backend.Domains.Role.Application.DI;
using Backend.Persistence.Sql.Context;
using Utils.EntityFramework.Application.Extensions;
using Utils.Mediator.Application.DI;
using Utils.Mediator.Application.Extensions;
using Utils.Serilog.Application.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule(new SerilogModule("Backend"));
            containerBuilder.RegisterModule(new MediatorModule(Assembly.GetExecutingAssembly()));

            containerBuilder.RegisterModule(new RoleModule());
        }
    );
builder.Services.RegisterDbContext<CoreDbContext>("backend");

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options => options.SupportedSubmitMethods());

await app.RunWithEventAsync().ConfigureAwait(false);
