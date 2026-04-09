using MediatR;
using Microsoft.Extensions.Logging;

namespace Template.Application.UseCases.ProcessTemplateConditions.Commands;

public sealed class ProcessTemplateConditionsCommandHandler : IRequestHandler<ProcessTemplateCreatedCommand, Unit>
{
    private readonly ILogger<ProcessTemplateConditionsCommandHandler> _logger;

    public ProcessTemplateConditionsCommandHandler(Logger<ProcessTemplateConditionsCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task<Unit> Handle(ProcessTemplateCreatedCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing template conditions");
        return Unit.Value;
    }
}
