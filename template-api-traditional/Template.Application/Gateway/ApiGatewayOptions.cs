namespace Template.Application.Gateway;

public sealed class ApiGatewayOptions
{
    public const string SectionName = "ApiGateway";

    public string BaseUrl { get; set; } = string.Empty;
    public TimeSpan? RequestTimeout { get; set; }
}
