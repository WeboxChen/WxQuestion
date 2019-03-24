namespace Wei.Web.Framework.ExtJs
{
    public class ResponseMessageExt
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public object obj { get; set; }

        public static ResponseMessageExt Success(string msg = null, object obj = null)
        {
            return new ResponseMessageExt() { success = true, msg = msg, obj = obj };
        }
        public static ResponseMessageExt Error(string msg = "", object obj = null)
        {
            return new ResponseMessageExt() { success = false, msg = msg, obj = obj };
        }
    }

    public class ResponseMessageExt<T>
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public T obj { get; set; }
    }
}
