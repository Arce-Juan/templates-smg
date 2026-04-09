namespace Template.Application.DTOs;

public record TemplateConditionsDto
{
    public decimal InterestRate { get; init; }
    public int TermMonths { get; init; }
    public decimal MonthlyPayment { get; init; }
    public decimal TotalAmount { get; init; }
}
