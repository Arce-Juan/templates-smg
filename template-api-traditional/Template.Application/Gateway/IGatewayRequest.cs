namespace Template.Application.Gateway;

public interface IGatewayRequest<TResponse>
{
    string RouteName { get; }
    string RelativeUri { get; }
    GatewayVerb Verb { get; }
    IReadOnlyDictionary<string, string>? Query { get; }
    object? Body { get; }
}
