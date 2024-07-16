using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SynonymSearchApp_ApplicationCore.Handlers
{
    public sealed class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var result = new ProblemDetails();
            switch (exception)
            {           
                case ArgumentException argumentException:
                    result = new ProblemDetails
                    {
                        Type = argumentException.GetType().Name,
                        Title = "Arguments not valid",
                        Detail = argumentException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(argumentException, $"Exception occured : {argumentException.Message}");
                    break;
                case NotImplementedException notImplementedException:
                    result = new ProblemDetails
                    {
                        Type = notImplementedException.GetType().Name,
                        Title = "Method is not implemented",
                        Detail = notImplementedException.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    };
                    _logger.LogError(notImplementedException, $"Exception occured : {notImplementedException.Message}");
                    break;
                default:
                    result = new ProblemDetails
                    {
                        Type = exception.GetType().Name,
                        Title = "An unexpected error occurred",
                        Detail = exception.Message,
                        Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                    };
                    _logger.LogError(exception, $"Exception occured : {exception.Message}");
                    break;
            }
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken: cancellationToken);
            return true;
        }
    }
}
