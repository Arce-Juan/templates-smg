using Template.Application.Common;
using Template.Application.DTOs;
using Template.Application.Interfaces;
using Template.Domain.Entities;
using Template.Domain.Events;
using Template.Domain.Interfaces;
using MediatR;
using System.Net;

namespace Template.Application.UseCases.CreateTemplate.Commands;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Response<CreateTemplateResult>>
{
    private readonly ITemplateRepository _repository;
    private readonly IEventBus _eventBus;

    public CreateTemplateCommandHandler(
        ITemplateRepository repository,
        IEventBus eventBus)
    {
        _repository = repository;
        _eventBus = eventBus;
    }

    public async Task<Response<CreateTemplateResult>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var clientId = request.ClientDocumentNumber.Trim();

        var id = Guid.NewGuid();
        var template = TemplateEntitie.Create();

        await _repository.AddAsync(template, cancellationToken).ConfigureAwait(false);

        var evt = new TemplateCreated(id, DateTimeOffset.UtcNow);

        await _eventBus.PublishAsync(evt, cancellationToken).ConfigureAwait(false);

        var result = new CreateTemplateResult { Id = id };
        return Response<CreateTemplateResult>.Ok(result, statusCode: HttpStatusCode.Accepted);
    }
}
