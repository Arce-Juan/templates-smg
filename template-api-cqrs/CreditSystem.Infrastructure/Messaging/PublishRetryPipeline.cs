using Polly;
using Polly.Retry;

namespace Template.Infrastructure.Messaging;

public static class PublishRetryPipeline
{
    public static ResiliencePipeline Pipeline { get; } = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType = DelayBackoffType.Exponential
        })
        .Build();
}
