namespace Template.Application.Gateway.Commands;

public sealed class PersonasListarCommand<TResponse> : GatewayCommandBase<TResponse>
{
    public override string RouteName => "personas";
    public override string RelativeUri => "/buscar";
    public override GatewayVerb Verb => GatewayVerb.Get;
}
