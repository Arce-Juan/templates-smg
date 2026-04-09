using Microsoft.AspNetCore.Mvc;
using Template.Api.Common;
using Template.Application.Common;
using Template.Application.Gateway.Dto;
using Template.Application.Interfaces;

namespace Template.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public sealed class PersonasController : BaseApiController
{
    private readonly IPersonasService _personasService;

    public PersonasController(IPersonasService personasService)
    {
        _personasService = personasService;
    }

    /// <summary>
    /// Obtiene la lista de personas (sin filtro) vía API Gateway (Ocelot).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Response<IReadOnlyList<PersonaGatewayDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response<IReadOnlyList<PersonaGatewayDto>>), StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _personasService.GetAllAsync(cancellationToken);
        return Result(result);
    }
}
