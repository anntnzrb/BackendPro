namespace BackendPro.Core.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException() { }

    public DuplicateEntityException(string message)
        : base(message) { }

    public DuplicateEntityException(string message, Exception innerException)
        : base(message, innerException) { }

    public DuplicateEntityException(string entityName, string fieldName, string value)
        : base($"{entityName} with {fieldName} '{value}' already exists.")
    {
        EntityName = entityName;
        FieldName = fieldName;
        Value = value;
    }

    public string EntityName { get; } = string.Empty;
    public string FieldName { get; } = string.Empty;
    public string Value { get; } = string.Empty;
}
