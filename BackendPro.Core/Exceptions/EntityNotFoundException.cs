namespace BackendPro.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException() { }

    public EntityNotFoundException(string message)
        : base(message) { }

    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }

    public EntityNotFoundException(string entityName, int id)
        : base($"{entityName} with ID {id} was not found.")
    {
        EntityName = entityName;
        EntityId = id;
    }

    public string EntityName { get; } = string.Empty;
    public int EntityId { get; }
}
