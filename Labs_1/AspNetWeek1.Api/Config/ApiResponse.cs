namespace AspNetWeek1.Api.Config
    public class ApiResponse<T>
    {
        //ma code de bao response cua api do co thanh cong khong
        int Code {set; get;} = 1000;
        //message cua phan hoi api
        string Message {set; get;} = string.Empty;
        //ket qua phan hoi api
        T Result {set; get;}
        ApiResponse<T> ApiResponse(int code, string message, T result)
        {
            Code = code;
            Message = message;
            Result = result;
        }
    }