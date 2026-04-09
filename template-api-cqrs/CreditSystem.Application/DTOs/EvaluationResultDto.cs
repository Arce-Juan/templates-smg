namespace Template.Application.DTOs;

public record EvaluationResultDto
{
    public bool Approved { get; init; }
    public string? Reason { get; init; }
}
