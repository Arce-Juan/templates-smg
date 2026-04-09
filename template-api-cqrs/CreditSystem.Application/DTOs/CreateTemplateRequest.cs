namespace Template.Application.DTOs;

public record CreateTemplateRequest
{
    public string ClientDocumentNumber { get; init; } = string.Empty;
    public decimal RequestedAmount { get; init; }
}
