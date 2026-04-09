using Template.Application.Common;
using Template.Application.Gateway.Dto;

namespace Template.Application.Interfaces;

public interface IPersonasService
{
    Task<Response<IReadOnlyList<PersonaGatewayDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}
