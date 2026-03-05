namespace HackerNewsDataCompiler.API.Domain.Models
{
    public class ResponseModel
    {
        public bool IsSuccess { get; private init; }
        public string? ErrorMessage { get; private init; }
        public int? ErrorStatusCode { get; private init; }

        private ResponseModel() { }

        public static ResponseModel Failure(int statusCode, string message) => new()
        {
            IsSuccess = false,
            ErrorStatusCode = statusCode,
            ErrorMessage = message
        };
    }

    public class ResponseModel<T>
    {
        public bool IsSuccess { get; private init; }
        public T? Data { get; private init; }
        public string? ErrorMessage { get; private init; }
        public int? ErrorStatusCode { get; private init; }

        private ResponseModel() { }

        public static ResponseModel<T> Success(T data) => new() { IsSuccess = true, Data = data };

        public static ResponseModel<T> Failure(int statusCode, string message) => new()
        {
            IsSuccess = false,
            ErrorStatusCode = statusCode,
            ErrorMessage = message
        };
    }
}
