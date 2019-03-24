using Newtonsoft.Json.Linq;
using System.Data;
using System.Web.Http;
using Wei.Core;
using Wei.Data;
using Wei.Services.Common;
using Wei.Services.Sys;
using Wei.Web.Framework.ApiControllers;
using Wei.Web.Framework.Controllers;
using Wei.Web.Framework.ExtJs;

namespace Wei.Web.API.Controllers
{
    public class CommonController : BaseApiController
    {
        private readonly ICommonService _commonService;
        private readonly IWorkContext _workContext;
        private readonly IDBService _dbService;

        public CommonController(ICommonService commonService
            , IWorkContext workContext
            , IDbContext dbContext
            , IDBService dbService
            )
        {
            this._commonService = commonService;
            this._workContext = workContext;
            this._dbService = dbService;
        }

        #region 通用视图
        [JsonCallback]
        [HttpPost]
        public object GetViewNonAlias(RequestPaging paras)
        {
            string viewname = paras.ExtraView;
            string filter = GetFilterNoAlias(paras.Filter);
            string sort = string.IsNullOrEmpty(paras.Sort) ? string.Format(" id {0} ", paras.Direction) : GetSortNoAlias(paras.Sort);
            int count, fcount;
            var table = _commonService.GetByViewNonAlias(viewname, paras.ExtraCols, out count, out fcount, paras.Start, paras.Limit, filter, sort);
            return new
            {
                total = fcount,
                data = table
            };
        }
        #endregion


        #region 基础数据增删改查
        [HttpPost]
        [JsonCallback]
        public object Read(RequestPaging request)
        {
            int count, fcount;
            string filter = GetFilterNoAlias(request.Filter);
            string sort = string.IsNullOrEmpty(request.Sort) ? string.Format(" id {0} ", request.Direction) : GetSortNoAlias(request.Sort);
            var data = _commonService.GetByViewNonAlias(request.ExtraView, out count, out fcount, request.Start, request.Limit, filter, sort);

            foreach (DataColumn column in data.Columns)
            {
                column.ColumnName = column.ColumnName.ToLower();
            }
            return new
            {
                total = fcount,
                data = data
            };
        }

        [HttpPost]
        [JsonCallback]
        [ActionLog(Wei.Core.Domain.Logging.ActionRecordType.Create)]
        public object Create(object obj, string tblname)
        {
            if (string.IsNullOrEmpty(tblname))
                return new { success = false, msg = "参数错误！【tblname】" };

            var table = this._dbService.GetDBTable(tblname);
            //string[] cols = _commonService.GetColumns(tblname);
            if (obj is JArray)
            {
                JArray jarray = obj as JArray;
                foreach (JObject model in jarray)
                    this._commonService.InsertEntity(tblname, model, table);
            }
            else
            {
                JObject model = obj as JObject;
                this._commonService.InsertEntity(tblname, model, table);
            }
            return new { success = true };
        }

        [HttpPost]
        [JsonCallback]
        [ActionLog(Wei.Core.Domain.Logging.ActionRecordType.Update)]
        public object Update(object obj, string tblname)
        {
            if (string.IsNullOrEmpty(tblname))
                return new { success = false, msg = "参数错误！【tblname】" };

            var table = this._dbService.GetDBTable(tblname);
            //string[] cols = _commonService.GetColumns(tblname);
            string msg = "";
            if (obj is JArray)
            {
                JArray jarray = obj as JArray;
                foreach (JObject model in jarray)
                {
                    int id = base.GetPropertyValue<int>(model, "id");
                    if (id == default(int) || id <= 0)
                    {
                        msg += "参数错误！【id】";
                        continue;
                    }

                    this._commonService.UpdateEntity(tblname, id, model, table);
                }
            }
            else
            {
                JObject model = obj as JObject;
                int id = base.GetPropertyValue<int>(model, "id");
                if (id == default(int) || id <= 0)
                    return new { success = false, msg = "参数错误！【id】" };

                this._commonService.UpdateEntity(tblname, id, model, table);
            }

            return new { success = string.IsNullOrEmpty(msg), msg = msg };
        }

        [HttpPost]
        [JsonCallback]
        [ActionLog(Wei.Core.Domain.Logging.ActionRecordType.Delete)]
        public object Destroy(object obj, string tblname)
        {
            if (string.IsNullOrEmpty(tblname))
                return new { success = false, msg = "参数错误！【tblname】" };

            string msg = "";
            if (obj is JArray)
            {
                JArray jarray = obj as JArray;
                foreach (JObject model in jarray)
                {
                    int id = base.GetPropertyValue<int>(model, "id");
                    if (id == default(int) || id <= 0)
                    {
                        msg += "参数错误！【id】";
                        continue;
                    }

                    this._commonService.DeleteEntity(tblname, id);
                }
            }
            else
            {
                JObject model = obj as JObject;
                int id = base.GetPropertyValue<int>(model, "id");
                if (id == default(int) || id <= 0)
                    return new { success = false, msg = "参数错误！【id】" };

                this._commonService.DeleteEntity(tblname, id);
            }

            return new { success = string.IsNullOrEmpty(msg), msg = msg };
        }
        #endregion
    }
}
