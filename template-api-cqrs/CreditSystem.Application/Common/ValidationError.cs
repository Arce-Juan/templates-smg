namespace Template.Application.Common;

public record ValidationError(string PropertyName, string Message, string? Code = null);
