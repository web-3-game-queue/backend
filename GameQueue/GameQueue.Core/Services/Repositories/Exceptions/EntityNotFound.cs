namespace GameQueue.Core.Contracts.Services.Repositories.Exceptions;

public class EntityNotFound : Exception
{
    public EntityNotFound(Type entityType, int entityId)
        : base(string.Format("Not found; entity: \"{0}\", id: {1}", entityType.ToString(), entityId)) { }
}
