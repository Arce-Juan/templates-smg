using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Template.Application.Gateway;

namespace Template.Infrastructure.Gateway;

public sealed class ApiGatewayClient : IApiGatewayClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApiGatewayOptions _options;

    public ApiGatewayClient(IHttpClientFactory httpClientFactory, IOptions<ApiGatewayOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<GatewayResponse<TResponse>> SendAsync<TResponse>(
        IGatewayRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.BaseUrl))
            throw new InvalidOperationException("ApiGateway:BaseUrl no está configurado.");

        var uri = BuildUri(request);
        using var message = new HttpRequestMessage(MapMethod(request.Verb), uri);

        if (request.Body is not null && request.Verb is not GatewayVerb.Get)
        {
            message.Content = JsonContent.Create(request.Body, options: JsonOptions);
        }

        var client = _httpClientFactory.CreateClient("ApiGateway");
        if (_options.RequestTimeout is { } t)
            client.Timeout = t;

        using var response = await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        var raw = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return GatewayResponse<TResponse>.Fail(response.StatusCode, raw);

        if (string.IsNullOrWhiteSpace(raw))
            return GatewayResponse<TResponse>.Ok(response.StatusCode, default);

        try
        {
            var data = JsonSerializer.Deserialize<TResponse>(raw, JsonOptions);
            return GatewayResponse<TResponse>.Ok(response.StatusCode, data);
        }
        catch (JsonException)
        {
            return GatewayResponse<TResponse>.Fail(
                response.StatusCode,
                $"Respuesta no JSON válida para {typeof(TResponse).Name}: {Truncate(raw)}");
        }
    }

    private Uri BuildUri<TResponse>(IGatewayRequest<TResponse> request)
    {
        var baseUri = _options.BaseUrl.TrimEnd('/');
        var path = $"{request.RouteName.Trim('/')}/{request.RelativeUri.TrimStart('/')}";
        var builder = new UriBuilder($"{baseUri}/{path}");

        if (request.Query is { Count: > 0 })
        {
            var qs = new StringBuilder();
            var first = true;
            foreach (var kv in request.Query)
            {
                if (!first) qs.Append('&');
                first = false;
                qs.Append(Uri.EscapeDataString(kv.Key));
                qs.Append('=');
                qs.Append(Uri.EscapeDataString(kv.Value));
            }

            builder.Query = qs.ToString();
        }

        return builder.Uri;
    }

    private static HttpMethod MapMethod(GatewayVerb verb) => verb switch
    {
        GatewayVerb.Get => HttpMethod.Get,
        GatewayVerb.Post => HttpMethod.Post,
        GatewayVerb.Put => HttpMethod.Put,
        GatewayVerb.Patch => HttpMethod.Patch,
        GatewayVerb.Delete => HttpMethod.Delete,
        _ => HttpMethod.Get
    };

    private static string Truncate(string s, int max = 500) =>
        s.Length <= max ? s : s[..max] + "…";
}
