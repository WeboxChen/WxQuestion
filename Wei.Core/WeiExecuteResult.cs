namespace Wei.Core
{
    public class WeiExecuteResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Obj { get; set; }
    }

    public class WeiExecuteResult<T>: WeiExecuteResult
    {
        public new T Obj { get; set; }
    }

    public static class WeiExecuteResultHelper
    {
        public static WeiExecuteResult Success(string message = null, object obj = null)
        {
            return new WeiExecuteResult() { Success = true, Message = message, Obj = obj };
        }
        public static WeiExecuteResult Failed(string message = null, object obj = null)
        {
            return new WeiExecuteResult() { Success = false, Message = message, Obj = obj };
        }

        public static WeiExecuteResult Success<T>(string message = null, T obj = default(T))
        {
            return new WeiExecuteResult() { Success = true, Message = message, Obj = obj };
        }
        public static WeiExecuteResult Failed<T>(string message = null, T obj = default(T))
        {
            return new WeiExecuteResult<T>() { Success = false, Message = message, Obj = obj };
        }
    }
}
