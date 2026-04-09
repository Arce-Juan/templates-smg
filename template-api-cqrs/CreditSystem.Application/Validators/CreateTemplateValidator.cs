using Template.Application.Common;
using Template.Application.UseCases.CreateTemplate.Commands;
using FluentValidation;

namespace Template.Application.Validators;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateCommand>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.ClientDocumentNumber)
            .NotEmpty()
            .WithMessage(ErrorMessage.CreateTemplate.ClientDocumentNumberRequired);
    }
}
