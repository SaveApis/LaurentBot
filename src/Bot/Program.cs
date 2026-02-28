// See https://aka.ms/new-console-template for more information

using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.Mediator.Application.DI;
using Utils.Mediator.Application.Extensions;
using Utils.Serilog.Application.DI;

var builder = Host.CreateApplicationBuilder();

builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder =>
    {
        containerBuilder.RegisterModule(new SerilogModule("Bot"));
        containerBuilder.RegisterModule(new MediatorModule(Assembly.GetExecutingAssembly()));
    }
);

var app = builder.Build();

await app.RunWithEventAsync().ConfigureAwait(false);
