using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wei.Web.Framework.ExtJs
{
    public class ResponseMessage<T>
    {
        public bool success { get; set; }
        public string msg { get; set; }
        public T obj { get; set; }
    }
}
