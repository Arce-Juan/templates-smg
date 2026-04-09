namespace Template.Application.Gateway;

public abstract class GatewayCommandBase<TResponse> : IGatewayRequest<TResponse>
{
    public abstract string RouteName { get; }
    public abstract string RelativeUri { get; }
    public abstract GatewayVerb Verb { get; }
    public virtual IReadOnlyDictionary<string, string>? Query => null;
    public virtual object? Body => null;
}
