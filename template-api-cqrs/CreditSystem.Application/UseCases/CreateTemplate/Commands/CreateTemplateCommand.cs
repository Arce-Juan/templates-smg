using Template.Application.Common;
using Template.Application.DTOs;
using MediatR;

namespace Template.Application.UseCases.CreateTemplate.Commands;

public record CreateTemplateCommand(
    string ClientDocumentNumber
) : IRequest<Response<CreateTemplateResult>>
{
    public static CreateTemplateCommand From(CreateTemplateRequest request) =>
        new(request.ClientDocumentNumber);
}
