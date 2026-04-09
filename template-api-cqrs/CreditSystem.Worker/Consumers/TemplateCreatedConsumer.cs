using MassTransit;
using MediatR;
using Template.Application.UseCases.ProcessTemplateConditions.Commands;
using Template.Domain.Events;

namespace Template.Worker.Consumers;

public class TemplateCreatedConsumer : IConsumer<TemplateCreated>
{
    private readonly IMediator _mediator;

    public TemplateCreatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Consume(ConsumeContext<TemplateCreated> context) =>
        _mediator.Send(ProcessTemplateCreatedCommand.From(new TemplateConditionsCalculated(new DateTimeOffset())), context.CancellationToken);
}
