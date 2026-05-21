namespace AspNetWeek1.Api.Exceptions;
public class AppException : Exception
{
    private ErrorCode errorCode {get; set;}
    public AppException(ErrorCode errorCode, string message) : base(message)
    {
        this.errorCode = errorCode;
    }
}