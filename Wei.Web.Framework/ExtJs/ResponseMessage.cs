namespace Wei.Web.Framework.ExtJs
{
    public class ResponseMessage
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object obj { get; set; }

        public static ResponseMessage Success(string msg = null, object obj = null)
        {
            return new ResponseMessage() { success = true, msg = msg, obj = obj };
        }
        public static ResponseMessage Error(string msg = "", object obj = null)
        {
            return new ResponseMessage() { success = false, msg = msg, obj = obj };
        }
    }

    public class ResponseMessage<T>
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public T obj { get; set; }
    }
}
