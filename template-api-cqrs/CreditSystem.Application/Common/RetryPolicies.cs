using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Template.Application.Common;

public static class RetryPolicies
{
    public static ResiliencePipeline<T> GetServiceCallPipeline<T>(ILogger logger) => new ResiliencePipelineBuilder<T>()
        .AddRetry(new RetryStrategyOptions<T>
        {
            ShouldHandle = new PredicateBuilder<T>().Handle<Exception>(),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(1),
            BackoffType = DelayBackoffType.Constant,
            OnRetry = args =>
            {
                var ex = args.Outcome.Exception;
                var attempt = args.AttemptNumber + 1;
                logger.LogWarning(ex, "External service call retry {Attempt} of 3 after error: {Message}", attempt, ex?.Message ?? "Unknown");
                return ValueTask.CompletedTask;
            }
        })
        .Build();
}
