using Template.Application.Common;
using Template.Domain.Enums;

namespace Template.Application.DTOs;

public record TemplateDto
{
    public Guid Id { get; init; }
    public string ClientId { get; init; } = string.Empty;
    public decimal RequestedAmount { get; init; }
    public TemplateStatus Status { get; init; }
    public string StatusName => EnumDisplayHelper.GetDisplayName(Status);
    public EvaluationResultDto? Eligibility { get; init; }
    public EvaluationResultDto? Risk { get; init; }
    public TemplateConditionsDto? Conditions { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
}
