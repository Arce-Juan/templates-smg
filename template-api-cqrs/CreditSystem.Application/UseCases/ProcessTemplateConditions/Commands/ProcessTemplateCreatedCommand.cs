using Template.Domain.Events;
using MediatR;

namespace Template.Application.UseCases.ProcessTemplateConditions.Commands;

public record ProcessTemplateCreatedCommand(
) : IRequest<Unit>
{
    public static ProcessTemplateCreatedCommand From(TemplateConditionsCalculated e) => new();
}
