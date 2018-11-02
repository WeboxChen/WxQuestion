using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using Wei.Core;
using Wei.Core.Domain.Logging;
using Wei.Core.Infrastructure;
using Wei.Services.Logging;

namespace Wei.Web.Framework.Controllers
{
    public class ActionLogAttribute : ActionFilterAttribute
    {
        // 1: 新增    2: 更新   -1: 删除  3: 导入   4: 文件上传
        private int _operateType;
        public ActionLogAttribute(ActionRecordType operateType)
        {
            this._operateType = (int)operateType;
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            
            if (IsSuccessResponse(context.Response))
                SaveActionLog(context.Request);
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool IsSuccessResponse(HttpResponseMessage response)
        {
            bool issuccess = false;
            if (response == null || response.Content == null) return issuccess;
            Stream stream = response.Content.ReadAsStreamAsync().Result;
            StreamReader reader = new StreamReader(stream);
            var result = reader.ReadToEnd();
            // 如果成功，记录操作日志
            JObject jobj = JObject.Parse(result);

            JToken token;
            if (jobj.TryGetValue("success", out token))
            {
                if (token.Type == JTokenType.Boolean && token.Value<bool>())
                    issuccess = true;
            }
            stream.Seek(0, SeekOrigin.Begin);
            return issuccess;
        }

        private void SaveActionLog(HttpRequestMessage request)
        {
            int objType = HttpContext.Current.Request.Params["_objType"].ToInt();
            int objId = HttpContext.Current.Request.Params["_objId"].ToInt();
            string objName = HttpContext.Current.Request.Params["_objName"];
            if (objId <= 0 || objType <= 0)         // 没有操作对象，不执行
                return;
            string payload = "";
            // 开始记录日志
            using (StreamReader reader = new StreamReader(request.Content.ReadAsStreamAsync().Result))
            {
                payload = reader.ReadToEnd(); // payload 值
            }
            // HttpContext.Current.Request.Params    // post url值
            // HttpContext.Current.Request.Form     // form 提交值
            var workcontext = EngineContext.Current.Resolve<IWorkContext>();
            var logservice = EngineContext.Current.Resolve<ILogger>();

            ActionRecord log = new ActionRecord() {
                Type = this._operateType,
                Operator = workcontext.CurrentUser.UserName,
                OperatorId = workcontext.CurrentUser.Id,
                OperateTime = DateTime.Now,
                Payload = payload,
                RequestParas = request.RequestUri.Query,
                ObjId = objId,
                ModuleName = objName,
                Info1 = objType.ToString()
            };

            // 写入操作视图
            string viewname = HttpContext.Current.Request.Params["extraView"];
            if (!string.IsNullOrEmpty(viewname))
                log.ViewName = viewname;
            // 写入操作存储过程
            string procname = HttpContext.Current.Request.Params["procname"];
            if (!string.IsNullOrEmpty(procname))
                log.ProcName = procname;

            logservice.InsertActionLog(log);
        }
    }
}
