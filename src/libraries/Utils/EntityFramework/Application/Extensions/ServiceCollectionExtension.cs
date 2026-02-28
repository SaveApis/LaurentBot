using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Utils.EntityFramework.Infrastructure.Persistence.Sql.Context;

namespace Utils.EntityFramework.Application.Extensions;

public static class ServiceCollectionExtension
{
    extension(IServiceCollection collection)
    {
        public void RegisterDbContext<TContext>(string selector) where TContext : BaseDbContext<TContext>
        {
            collection.AddDbContextFactory<TContext>((provider, builder) =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var environment = provider.GetRequiredService<IHostEnvironment>();
                    
                    var host = configuration.GetValue<string>($"Database:{selector}:Host") ?? throw new InvalidOperationException($"Database:{selector}:Host is not configured.");
                    var port = configuration.GetValue<int>($"Database:{selector}:Port");
                    var database = configuration.GetValue<string>($"Database:{selector}:Database") ?? throw new InvalidOperationException($"Database:{selector}:Database is not configured.");
                    var username = configuration.GetValue<string>($"Database:{selector}:Username") ?? throw new InvalidOperationException($"Database:{selector}:Username is not configured.");
                    var password = configuration.GetValue<string>($"Database:{selector}:Password") ?? throw new InvalidOperationException($"Database:{selector}:Password is not configured.");

                    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                    {
                        ApplicationName = "Laurent Bot",
                        Host = host,
                        Port = port,
                        Database = database,
                        Username = username,
                        Password = password,
                        Pooling = true,
                        Multiplexing = false,
                        IncludeFailedBatchedCommand = true,
                        LogParameters = environment.IsDevelopment(),
                        IncludeErrorDetail = environment.IsDevelopment(),
                        BrowsableConnectionString = environment.IsDevelopment(),
                    };

                    builder.EnableDetailedErrors();
                    builder.EnableSensitiveDataLogging(environment.IsDevelopment());
                    builder.UseNpgsql(connectionStringBuilder.ToString(), optionsBuilder =>
                        {
                            optionsBuilder.EnableRetryOnFailure();
                            optionsBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            optionsBuilder.UseRelationalNulls();
                        }
                    );
                }
            );
        }
    }
}
