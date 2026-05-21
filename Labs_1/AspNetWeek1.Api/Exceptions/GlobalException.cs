using AspNetWeek1.Api.Config;
using Microsoft.AspNetCore.Diagnostics;

namespace AspNetWeek1.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{   
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        var response = new ApiResponse<object>();
        if(exception is AppException appEx)
        {
            
        }
        else
        {
            response.Code  = (int)ErrorCode.Uncategorized;
            response.Message = "An internal server error occured.";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}