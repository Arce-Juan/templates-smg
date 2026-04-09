using Template.Application.Common;
using Template.Application.Gateway;
using Template.Application.Gateway.Commands;
using Template.Application.Gateway.Dto;
using Template.Application.Interfaces;

namespace Template.Application.Services;

public sealed class PersonasService : IPersonasService
{
    private readonly IApiGatewayClient _gateway;

    public PersonasService(IApiGatewayClient gateway)
    {
        _gateway = gateway;
    }

    public async Task<Response<IReadOnlyList<PersonaGatewayDto>>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var cmd = new PersonasListarCommand<IReadOnlyList<PersonaGatewayDto>>();
        var gw = await _gateway.SendAsync(cmd, cancellationToken).ConfigureAwait(false);

        if (!gw.IsSuccess)
            return GatewayResponseMapper.ToApplicationResponse(gw);

        var list = gw.Data ?? Array.Empty<PersonaGatewayDto>();
        return Response<IReadOnlyList<PersonaGatewayDto>>.Ok(list);
    }
}
