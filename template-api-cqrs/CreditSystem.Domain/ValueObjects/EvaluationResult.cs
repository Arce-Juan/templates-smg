namespace Template.Domain.ValueObjects;

public record EvaluationResult
{
    public bool Approved { get; init; }
    public string? Reason { get; init; }

    private EvaluationResult() { }

    public EvaluationResult(bool approved, string? reason = null)
    {
        Approved = approved;
        Reason = reason;
    }
}
