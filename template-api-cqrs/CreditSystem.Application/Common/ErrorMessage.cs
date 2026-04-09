namespace Template.Application.Common;

public static class ErrorMessage
{
    public static class Template
    {
        public const string NotFound = "Template not found.";
        public const string ClientIdCannotBeEmpty = "Client ID cannot be empty.";
        public const string RequestedAmountMustBeGreaterThanZero = "Requested amount must be greater than zero.";
    }

    public static class CreateTemplate
    {
        public const string ClientDocumentNumberRequired = "Client document number is required.";
        public const string RequestedAmountMustBeGreaterThanZero = "Requested amount must be greater than zero.";
        public const string ClientHasTemplateInProgress = "The client already has a template in progress.";
    }

    public static class Validation
    {
        public const string Title = "Error de validación";
        public const string RequestValidationFailed = "One or more validation errors occurred.";
    }
}
