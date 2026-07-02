namespace StationeryStore.Mvc.Exception;

public class AppException : System.Exception
{
    public ErrorCode ErrorCode {get; set;}
    public AppException(ErrorCode errorCode) : base(errorCode.GetMessage())
    {
        ErrorCode = errorCode;
    }
    
}