namespace GameQueue.Host.Exceptions;

public class InvalidContentTypeException : Exception
{
    public InvalidContentTypeException(string contentType, string[] allowedContentTypes)
        : base(string.Format("Invalid content type: \"{0}\", allowed content types: {1}", contentType, allowedContentTypes)) { }
}
