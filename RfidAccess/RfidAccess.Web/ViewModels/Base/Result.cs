namespace RfidAccess.Web.ViewModels.Base
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }

        public bool IsFailed { get; protected set; }

        public string? Message { get; protected set; }

        protected Result() { }

        public Result(string message)
        {
            Message = message;
            IsSuccess = false;
            IsFailed = true;
        }

        private Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
            IsFailed = !isSuccess;
        }

        public static Result Success => new Result(true);

        public static Result Failure(string message)
        {
            return new Result(message);
        }
    }

    public class Result<T> : Result
    {
        public T? Value { get; set; }

        public Result(string message)
            : base(message)
        {
            Value = default;
        }

        public Result(T value)
        {
            Value = value;
            IsSuccess = true;
            IsFailed = false;
        }

        public static Result Failure<T>(string message)
        {
            return new Result<T>(message);
        }

        public static Result Success(T value)
        {
            return new Result<T>(value);
        }
    }
}
