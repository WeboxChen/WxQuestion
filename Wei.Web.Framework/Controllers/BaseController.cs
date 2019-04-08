﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Wei.Core;
using Wei.Core.Infrastructure;
using Wei.Services.Logging;
using Wei.Web.Framework.ExtJs;
using Wei.Web.Framework.UI;

namespace Wei.Web.Framework.Controllers
{
    [UserPermission]
    /// <summary>
    /// Base controller
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }


        ///// <summary>
        ///// Get active store scope (for multi-store configuration mode)
        ///// </summary>
        ///// <param name="storeService">Store service</param>
        ///// <param name="workContext">Work context</param>
        ///// <returns>Store ID; 0 if we are in a shared mode</returns>
        //public virtual int GetActiveStoreScopeConfiguration(IStoreService storeService, IWorkContext workContext)
        //{
        //    //ensure that we have 2 (or more) stores
        //    if (storeService.GetAllStores().Count < 2)
        //        return 0;


        //    var storeId = workContext.CurrentUser.GetAttribute<int>(SystemUserAttributeNames.AdminAreaStoreScopeConfiguration);
        //    var store = storeService.GetStoreById(storeId);
        //    return store != null ? store.Id : 0;
        //}


        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        protected void LogException(Exception exc)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var logger = EngineContext.Current.Resolve<ILogger>();

            var user = workContext.CurrentUser;
            logger.Error(exc.Message, exc, user);
        }
        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                LogException(exception);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }
        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("nop.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }



        ///// <summary>
        ///// Add locales for localizable entities
        ///// </summary>
        ///// <typeparam name="TLocalizedModelLocal">Localizable model</typeparam>
        ///// <param name="languageService">Language service</param>
        ///// <param name="locales">Locales</param>
        //protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales) where TLocalizedModelLocal : ILocalizedModelLocal
        //{
        //    AddLocales(languageService, locales, null);
        //}
        ///// <summary>
        ///// Add locales for localizable entities
        ///// </summary>
        ///// <typeparam name="TLocalizedModelLocal">Localizable model</typeparam>
        ///// <param name="languageService">Language service</param>
        ///// <param name="locales">Locales</param>
        ///// <param name="configure">Configure action</param>
        //protected virtual void AddLocales<TLocalizedModelLocal>(ILanguageService languageService, IList<TLocalizedModelLocal> locales, Action<TLocalizedModelLocal, int> configure) where TLocalizedModelLocal : ILocalizedModelLocal
        //{
        //    foreach (var language in languageService.GetAllLanguages(true))
        //    {
        //        var locale = Activator.CreateInstance<TLocalizedModelLocal>();
        //        locale.LanguageId = language.Id;
        //        if (configure != null)
        //        {
        //            configure.Invoke(locale, locale.LanguageId);
        //        }
        //        locales.Add(locale);
        //    }
        //}

        #region extension
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

        protected string GetFilter(string filter)
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
        protected string GetSort(string sort)
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
        #endregion
    }
}