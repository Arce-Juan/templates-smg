using Template.Application.Common;
using Template.Application.DTOs;
using Template.Application.UseCases.CreateTemplate.Commands;
using Template.Application.UseCases.GetTemplate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Template.Api.Controllers;

[ApiController]
[Route("api/templates")]
[Authorize]
public class TemplateController : BaseApiController
{
    private readonly IMediator _mediator;

    public TemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a template. The template is processed asynchronously.
    /// </summary>
    /// <response code="202">Request accepted. Location header contains the created resource URI.</response>
    /// <response code="400">Invalid input.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<CreateTemplateResult>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(Response<CreateTemplateResult>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTemplateRequest request, CancellationToken cancellationToken)
    {
        var command = CreateTemplateCommand.From(request);
        var response = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        return Result(response);
    }

    /// <summary>
    /// Gets the current status and detail of a template.
    /// </summary>
    /// <response code="200">Template found. Includes current status and results.</response>
    /// <response code="404">Template not found.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<TemplateDto?>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetTemplateQuery(id);
        var response = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
        return Result(response);
    }
}
