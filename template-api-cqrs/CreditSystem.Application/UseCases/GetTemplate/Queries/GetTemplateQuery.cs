using MediatR;
using Template.Application.Common;
using Template.Application.DTOs;

namespace Template.Application.UseCases.GetTemplate.Queries;

public record GetTemplateQuery(Guid Id) : IRequest<Response<TemplateDto>>;
