using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Wei.Web.Framework.ExtJs;

namespace Wei.Web.Framework.ApiControllers
{
    [WebApiExceptionFilter]
    //[UserApiAuthorize]
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// 获取查询条件字符串  [tname.cname]
        /// </summary>
        /// <param name="tname"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected string GetOperatorStr(string tname, FilterModel item)
        {
            string colname = "{0}";
            if (!string.IsNullOrEmpty(tname))
            {
                colname = "[" + tname + ".{0}]";
            }
            switch (item.Operator)
            {
                case "lt":
                case "<":
                    return string.Format(" and " + colname + " < '{1}' ", item.Property, item.Value);
                case "gt":
                case ">":
                    return string.Format(" and " + colname + " > '{1}' ", item.Property, item.Value);
                case "lteq":
                case "<=":
                    return string.Format(" and " + colname + " <= '{1}' ", item.Property, item.Value);
                case "gteq":
                case ">=":
                    return string.Format(" and " + colname + " >= '{1}' ", item.Property, item.Value);
                case "noteq":
                case "!=":
                    return string.Format(" and " + colname + " != '{1}' ", item.Property, item.Value);
                case "like":
                    return string.Format(" and " + colname + " like '%{1}%' ", item.Property, item.Value);
                case "in":
                    string str = "";
                    if (item.Value is JArray)
                    {
                        var filterarr = (JArray)item.Value;
                        str = string.Format(" and " + colname + " in ({1}) ", item.Property, "'" + string.Join("','", filterarr) + "'");
                        //}
                    }
                    return str; ;
                case "eq":
                case "=":
                default:
                    DateTime time;
                    if (DateTime.TryParse(item.Value.ToString(), out time))
                        return string.Format(" and convert(varchar(10), " + colname + ", 120) = '{1}' ", item.Property, time.ToString("yyyy-MM-dd"));

                    return string.Format(" and " + colname + " = '{1}' ", item.Property, item.Value);
            }
        }

        protected string GetFilterNoAlias(string filter)
        {
            string result = "";
            if (!string.IsNullOrEmpty(filter))
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<FilterModel>>(filter);
                foreach (var model in obj)
                {
                    result += GetOperatorStr("", model);
                }
            }
            return result;
        }
        protected string GetSortNoAlias(string sort)
        {
            string result = "";
            if (!string.IsNullOrEmpty(sort))
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<SortModel>>(sort);
                // filter
                foreach (var item in obj)
                {
                    result += string.Format(" {0} {1},", item.Property, item.Direction);
                }
            }
            return result.TrimEnd(',');
        }

        /// <summary>
        /// 获取jobject值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jobj"></param>
        /// <param name="pname"></param>
        /// <returns></returns>
        protected T GetPropertyValue<T>(JObject jobj, string pname)
        {
            var prop = jobj.Property(pname);
            if (prop != null)
                return prop.Value.Value<T>();
            return default(T);
        }
    }
}
