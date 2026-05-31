using Microsoft.AspNetCore.Diagnostics;

namespace StationeryStore.Mvc.Exception;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger; 
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        System.Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error occured: {Message}", exception.Message);
        var errorCode = exception is AppException appEx ? appEx.ErrorCode : ErrorCode.UNCATEGORIEZED;

        httpContext.Response.StatusCode = (int) errorCode.GetHttpStatus();        

        await httpContext.Response.WriteAsJsonAsync(new
        {
            Code = errorCode.GetCode(),
            Message = errorCode.GetMessage(),
            StatusCode = errorCode.GetHttpStatus()
        }, cancellationToken);
        return true;
    }
}