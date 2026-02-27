// See https://aka.ms/new-console-template for more information

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.Serilog.Application.DI;

var builder = Host.CreateApplicationBuilder();

builder.ConfigureContainer(new AutofacServiceProviderFactory(), containerBuilder => containerBuilder.RegisterModule(new SerilogModule("Bot")));

var app = builder.Build();

await app.StartAsync().ConfigureAwait(false);
await app.WaitForShutdownAsync().ConfigureAwait(false);
