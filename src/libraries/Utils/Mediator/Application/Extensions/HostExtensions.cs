using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.Mediator.Application.Events;

namespace Utils.Mediator.Application.Extensions;

public static class HostExtensions
{
    extension(IHost host)
    {
        public async Task RunWithEventAsync()
        {
            var scope = host.Services.CreateAsyncScope();


            await host.StartAsync().ConfigureAwait(false);
            await using (var _ = scope.ConfigureAwait(false))
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(new ApplicationStartedEvent()).ConfigureAwait(false);
            }
            await host.WaitForShutdownAsync().ConfigureAwait(false);
        }
    }
}
