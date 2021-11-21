namespace lolappAPI.Types
{
    public class ApiResponse<T>
    {
        public int HttpStatusCode { get; private set; }
        public string Message { get; private set; }
        public int ItemCount { get; private set; }
        public T Result { get; private set; }
        public ApiResponse(int httpStatusCode, string message, int itemCount, T result)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
            ItemCount = itemCount;
            Result = result;
        }
    }
}
