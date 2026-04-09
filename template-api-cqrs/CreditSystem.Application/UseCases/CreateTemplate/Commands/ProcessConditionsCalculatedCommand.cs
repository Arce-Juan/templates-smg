using Template.Domain.Events;
using MediatR;

namespace Template.Application.UseCases.CreateTemplate.Commands;

public record ProcessConditionsCalculatedCommand(
    DateTimeOffset OccurredAt
) : IRequest<Unit>
{
    public static ProcessConditionsCalculatedCommand From(TemplateConditionsCalculated e) => new(
        e.OccurredAt);
}
