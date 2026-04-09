using Template.Application.Interfaces;
using MassTransit;

namespace Template.Infrastructure.Messaging;

public class MassTransitEventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        await PublishRetryPipeline.Pipeline.ExecuteAsync(
            async ct => await _publishEndpoint.Publish(message, ct).ConfigureAwait(false),
            cancellationToken).ConfigureAwait(false);
    }
}
