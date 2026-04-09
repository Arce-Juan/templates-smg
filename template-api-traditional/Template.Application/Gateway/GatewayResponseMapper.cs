using System.Net;
using Template.Application.Common;

namespace Template.Application.Gateway;

/// <summary>
/// Convierte <see cref="GatewayResponse{T}"/> (transporte HTTP al API Gateway) en <see cref="Response{T}"/>
/// (contrato único de la capa de aplicación hacia controllers).
/// </summary>
public static class GatewayResponseMapper
{
    public static Response<T> ToApplicationResponse<T>(
        GatewayResponse<T> gatewayResponse,
        string failureMessage = "Error consultando API Gateway.")
    {
        if (!gatewayResponse.IsSuccess)
        {
            return Response<T>.Failure(
                message: failureMessage,
                type: "api-gateway-error",
                title: "Bad Gateway",
                detail: gatewayResponse.ErrorContent,
                statusCode: HttpStatusCode.BadGateway);
        }

        return Response<T>.Ok(gatewayResponse.Data!);
    }
}
