namespace GameQueue.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(Type entityType, int entityId)
        : base(string.Format("Not found; entity: \"{0}\", id: {1}", entityType.ToString(), entityId)) { }
}
