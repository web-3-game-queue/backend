using Microsoft.AspNetCore.Mvc.Filters;

namespace GameQueue.Backend.ExceptionFilters;

public class ExceptionFilter : IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context) => throw new NotImplementedException();
}
