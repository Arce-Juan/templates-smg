using Microsoft.AspNetCore.Mvc;
using Template.Api.Common;
using Template.Application.Common;
using Template.Application.Interfaces;
using Template.Application.ValueObjects;

namespace Template.Api.Controllers;

/// <summary>
/// Template management API endpoints for CRUD operations.
/// Provides functionality to create, read, update, and delete template entities.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TemplateController : BaseApiController
{
    private readonly ITemplateService _templateService;

    /// <summary>
    /// Initializes a new instance of the TemplateController.
    /// </summary>
    /// <param name="templateService">The template service for business logic operations.</param>
    public TemplateController(ITemplateService templateService)
    {
        _templateService = templateService;
    }

    /// <summary>
    /// Retrieves a specific template by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the template to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The template data if found, otherwise a not found response.</returns>
    /// <response code="200">Returns the requested template data.</response>
    /// <response code="400">Bad request if the provided ID is invalid.</response>
    /// <response code="404">Template not found with the specified ID.</response>
    /// <response code="500">Internal server error if an unexpected error occurs.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _templateService.GetByIdAsync(id, cancellationToken);
        return Result(result);
    }

    /// <summary>
    /// Retrieves all available templates from the system.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A list of all templates in the system.</returns>
    /// <response code="200">Returns the complete list of templates.</response>
    /// <response code="500">Internal server error if an unexpected error occurs.</response>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IReadOnlyList<TemplateDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<IReadOnlyList<TemplateDto>>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _templateService.GetAllAsync(cancellationToken);
        return Result(result);
    }

    /// <summary>
    /// Creates a new template in the system.
    /// </summary>
    /// <param name="template">The template data to create. ID will be auto-generated.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The newly created template data.</returns>
    /// <response code="201">Template successfully created and returned.</response>
    /// <response code="400">Bad request if the template data is invalid or missing required fields.</response>
    /// <response code="500">Internal server error if creation fails.</response>
    [HttpPost]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] TemplateDto template, CancellationToken cancellationToken = default)
    {
        var result = await _templateService.CreateAsync(template, cancellationToken);
        return Result(result);
    }

    /// <summary>
    /// Updates an existing template with new data.
    /// </summary>
    /// <param name="id">The unique identifier of the template to update.</param>
    /// <param name="template">The updated template data. Must include the ID in the body.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The updated template data.</returns>
    /// <response code="200">Template successfully updated.</response>
    /// <response code="400">Bad request if the template data is invalid or ID mismatch.</response>
    /// <response code="404">Template not found with the specified ID.</response>
    /// <response code="500">Internal server error if update fails.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Response<TemplateDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] TemplateDto template, CancellationToken cancellationToken = default)
    {
        var result = await _templateService.UpdateAsync(id, template, cancellationToken);
        return Result(result);
    }
}
