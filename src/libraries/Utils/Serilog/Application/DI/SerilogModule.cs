using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Utils.Serilog.Application.DI;

public class SerilogModule(string applicationName) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var collection = new ServiceCollection();

        collection.AddSerilog((provider, loggerConfiguration) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();

                var environment = provider.GetRequiredService<IHostEnvironment>();
                var isDevelopment = environment.IsDevelopment();

                var minimumLevels = configuration.GetSection("Logging:MinimumLevels").Get<Dictionary<string, LogEventLevel>>() ?? [];
                loggerConfiguration.MinimumLevel.Verbose();
                foreach (var (namespacePrefix, level) in minimumLevels)
                {
                    loggerConfiguration.MinimumLevel.Override(namespacePrefix.Replace('_', '.'), level);
                }

                loggerConfiguration.WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}");
                if (!isDevelopment)
                {
                    var token = configuration["BetterStack:Token"] ?? throw new InvalidOperationException("BetterStack token is not configured.");
                    var host = configuration["BetterStack:Host"] ?? throw new InvalidOperationException("BetterStack host is not configured.");

                    loggerConfiguration.WriteTo.BetterStack(token, $"https://{host}");
                }

                loggerConfiguration.Enrich.WithProperty("Application", applicationName);
                loggerConfiguration.Enrich.WithProperty("Host", Environment.MachineName);
            }
        );

        builder.Populate(collection);
    }
}
