using System;
using System.Web.Http.Filters;
using Wei.Core;
using Wei.Core.Infrastructure;
using Wei.Services.Logging;

namespace Wei.Web.Framework.Controllers
{
    /// <summary>
    /// 请求出错，保存日志
    /// </summary>
    public class WebApiExceptionFilterAttribute: ExceptionFilterAttribute
    {
        //重写基类的异常处理方法
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exc = actionExecutedContext.Exception;
            try
            {
                //log
                var logger = EngineContext.Current.Resolve<ILogger>();
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                logger.Error(exc.Message, exc, workContext.CurrentUser);
            }
            catch (Exception)
            {
                //don't throw new exception if occurs
            }
            base.OnException(actionExecutedContext);
        }
    }
}
