using MediatR;
using System.Net;
using Template.Application.Common;
using Template.Application.DTOs;
using Template.Domain.Interfaces;

namespace Template.Application.UseCases.GetTemplate.Queries;

public class GetTemplateQueryHandler : IRequestHandler<GetTemplateQuery, Response<TemplateDto>>
{
    private readonly ITemplateRepository _repository;

    public GetTemplateQueryHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<TemplateDto>> Handle(GetTemplateQuery request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (template == null)
            return Response<TemplateDto>.Failure(
                ErrorMessage.Template.NotFound,
                statusCode: HttpStatusCode.NotFound);

        var templateDto = new TemplateDto
        {
        };

        return Response<TemplateDto>.Ok(templateDto);
    }
}
