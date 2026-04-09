namespace Template.Application.Gateway;

public interface IApiGatewayClient
{
    Task<GatewayResponse<TResponse>> SendAsync<TResponse>(
        IGatewayRequest<TResponse> request,
        CancellationToken cancellationToken = default);
}
