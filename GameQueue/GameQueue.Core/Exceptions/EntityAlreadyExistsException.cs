namespace GameQueue.Core.Exceptions;

public class EntityAlreadyExistsException: Exception
{
    public EntityAlreadyExistsException(Type entityType, int entityId)
        : base(string.Format("Already exists; entity: \"{0}\", id: {1}", entityType.ToString(), entityId)) { }
}
