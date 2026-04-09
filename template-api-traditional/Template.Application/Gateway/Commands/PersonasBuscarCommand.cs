using Template.Application.Gateway;

namespace Template.Application.Gateway.Commands;

public sealed class PersonasBuscarCommand<TResponse> : GatewayCommandBase<TResponse>
{
    public PersonasBuscarCommand(string documento)
    {
        Documento = documento;
    }

    public string Documento { get; }
    public override string RouteName => "personas";
    public override string RelativeUri => "/buscar";
    public override GatewayVerb Verb => GatewayVerb.Get;
    public override IReadOnlyDictionary<string, string>? Query =>
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["documento"] = Documento
        };
}
