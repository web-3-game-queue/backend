using GameQueue.Core.Models;

namespace GameQueue.Core.Exceptions;

public class InvalidSearchMapsRequestStatusUpdateException : Exception
{
    public InvalidSearchMapsRequestStatusUpdateException(SearchMapsRequestStatus current, SearchMapsRequestStatus target)
        : base(string.Format("Status \"{0}\" can not be changed to \"{1}\"", current.ToString(), target.ToString())) { }
}
