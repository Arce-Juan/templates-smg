using System.ComponentModel.DataAnnotations;

namespace Template.Domain.Enums;

public enum TemplateStatus
{
    [Display(Name = "Created")]
    Created = 0,

    [Display(Name = "Validating Eligibility")]
    ValidatingEligibility = 1,

    [Display(Name = "Failed")]
    Failed = 9,

    [Display(Name = "Cancelled")]
    Cancelled = 11,
}
