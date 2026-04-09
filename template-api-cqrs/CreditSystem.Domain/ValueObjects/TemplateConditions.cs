
namespace Template.Domain.ValueObjects;

public record TemplateConditions
{
    public decimal InterestRate { get; init; }
    public int TermMonths { get; init; }
    public decimal MonthlyPayment { get; init; }
    public decimal TotalAmount { get; init; }

    private TemplateConditions() { }

    public TemplateConditions(decimal interestRate, int termMonths, decimal monthlyPayment, decimal totalAmount)
    {
        if (termMonths <= 0)
            throw new ArgumentException("Term months must be greater than zero", nameof(termMonths));
        InterestRate = interestRate;
        TermMonths = termMonths;
        MonthlyPayment = monthlyPayment;
        TotalAmount = totalAmount;
    }
}
