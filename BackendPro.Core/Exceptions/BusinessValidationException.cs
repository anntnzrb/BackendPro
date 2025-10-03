namespace BackendPro.Core.Exceptions;

public class BusinessValidationException : Exception
{
    public BusinessValidationException(string message)
        : base(message)
    {
    }

    public BusinessValidationException(string message, IReadOnlyDictionary<string, string[]> errors)
        : base(message) => Errors = errors;

    public BusinessValidationException() : base() { }

    public BusinessValidationException(string? message, Exception? innerException) : base(message, innerException) { }

    public IReadOnlyDictionary<string, string[]>? Errors { get; }
}
