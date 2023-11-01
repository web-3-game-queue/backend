namespace GameQueue.Core.Exceptions;

public class UpdateCommandEmptyException : Exception
{
    public UpdateCommandEmptyException() : base("Update command can not be empty") { }
}
