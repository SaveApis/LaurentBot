// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

var app = builder.Build();

await app.StartAsync().ConfigureAwait(false);
await app.WaitForShutdownAsync().ConfigureAwait(false);
