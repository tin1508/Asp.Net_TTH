using System.Net;

namespace StationeryStore.Mvc.Exception;

public enum ErrorCode
{
    UNCATEGORIEZED, 
    INVALID_KEY,
    INVALID_PRICE,
    NOT_FOUND,
    EXISTED_STATIONERY,
    EXISTED_CATEGORY,
    NOT_EXISTED_CATEGORY,
    NOT_EXISTED_STATIONERY,
    NOT_ENOUGH_QUANTITY,
    AT_LEAST_STATIONERY_WHEN_ORDERING
}
public static class ErrorCodeData
{
    private static readonly Dictionary<ErrorCode, (int Code, string Message, HttpStatusCode HttpStatus)> _map = new()
    {
        {ErrorCode.UNCATEGORIEZED, (999, "Uncategorize exception", HttpStatusCode.InternalServerError)},
        {ErrorCode.INVALID_KEY, (1001, "Keyword cannot be null or whitespace", HttpStatusCode.BadRequest)},
        {ErrorCode.NOT_FOUND, (1002, "The information is not found", HttpStatusCode.NotFound)},
        {ErrorCode.EXISTED_STATIONERY, (1003, "Stationery has already existed", HttpStatusCode.BadRequest)},
        {ErrorCode.NOT_EXISTED_CATEGORY, (1004, "Category has not existed", HttpStatusCode.BadRequest)},
        {ErrorCode.EXISTED_CATEGORY, (1005, "Category has already existed", HttpStatusCode.BadRequest)},
        {ErrorCode.NOT_EXISTED_STATIONERY, (1006, "Stationery has not existed", HttpStatusCode.NotFound)},
        {ErrorCode.NOT_ENOUGH_QUANTITY, (1007, "The quantity of stationeries are not enough!!!", HttpStatusCode.BadRequest)},
        {ErrorCode.AT_LEAST_STATIONERY_WHEN_ORDERING, (1007, "The order must have one stationery at least!!!", HttpStatusCode.BadRequest)}
    };
    public static int GetCode(this ErrorCode errorCode) => _map[errorCode].Code;
    public static string GetMessage(this ErrorCode errorCode) => _map[errorCode].Message;
    public static HttpStatusCode GetHttpStatus(this ErrorCode errorCode) => _map[errorCode].HttpStatus;
}