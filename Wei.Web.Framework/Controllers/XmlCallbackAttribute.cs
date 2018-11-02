using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace Wei.Web.Framework.Controllers
{
    /// <summary>
    /// WebApi 返回数据
    /// </summary>
    public class XmlCallbackAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            HttpContext.Current.Response.ContentType = "application/xml; charset=utf-8";
            
            base.OnActionExecuted(context);
        }
    }
}
