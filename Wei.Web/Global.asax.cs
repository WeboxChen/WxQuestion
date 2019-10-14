using FluentValidation.Mvc;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using StackExchange.Profiling;
using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Wei.Core;
using Wei.Core.Configuration;
using Wei.Core.Data;
using Wei.Core.Infrastructure;
using Wei.Data;
using Wei.Services.Logging;
using Wei.Web.Framework;
using Wei.Web.Framework.Mvc.Routes;

namespace Wei.Web
{
    public class Global : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "WeiXin", action = "Index" },
                new[] { "Wei.Web.Controllers" }
            );
        }

        public static void RegisterApiRoutes(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}"
            );
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        void Application_Start(object sender, EventArgs e)
        {

            #region 微信注册
            /* CO2NET 全局注册开始
             * 建议按照以下顺序进行注册
             */

            //设置全局 Debug 状态
            var isGLobalDebug = true;
            var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);

            //CO2NET 全局注册，必须！！
            IRegisterService register = RegisterService.Start(senparcSetting)
                                          .UseSenparcGlobal(false, null);

            /* 微信配置开始
             * 建议按照以下顺序进行注册
             */

            //设置微信 Debug 状态
            var isWeixinDebug = true;
            var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);

            //微信全局注册，必须！！
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting)
                .RegisterMpAccount(senparcWeixinSetting, "WXQuestion");
            // 注册微信AppId
            //AccessTokenContainer.RegisterAsync(WXinConfig.WeixinAppId, WXinConfig.WeixinAppSecret).Start();
            
            // 异步执行
            //new Thread(x => {
            //}).Start();
            #endregion

            //初始化Aoc容器
            EngineContext.Initialize(false);
            // 数据库不用初始化  CodeFirst 容易报错
            Database.SetInitializer<WeiObjectContext>(null);

            //RegisterRoutes(RouteTable.Routes);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //fluent数据验证设置
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;        // 属性值可以为null
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new WeiValidatorFactory()));
            // GlobalFilterCollection
            RegisterGlobalFilters(GlobalFilters.Filters);

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.

            //Task t = new Task(() =>
            //{
            //    var tlogger = EngineContext.Current.Resolve<ILogger>();
            //    try
            //    {
            //        ViewServices.Users.UserViewService uservice = new ViewServices.Users.UserViewService();
            //        uservice.SaveUserList(WXinConfig.WeixinAppId);
            //    }
            //    catch (Exception ex)
            //    {
            //        //var logger2 = EngineContext.Current.Resolve<ILogger>();
            //        tlogger.Error(ex.Message, ex, null);
            //    }
            //});
            //t.Start();
            var logger = EngineContext.Current.Resolve<ILogger>();

            logger.Information("Application started", null, null);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            if (webHelper.IsStaticResource(this.Request))
                return;

            //keep alive page requested (we ignore it to prevent creating a guest customer records)
            string keepAliveUrl = string.Format("{0}keepalive/index", webHelper.GetStoreLocation());
            if (webHelper.GetThisPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;

            //miniprofiler
            //MiniProfiler.Start();
            //HttpContext.Current.Items["wei.MiniProfilerStarted"] = true;
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //miniprofiler
            //var miniProfilerStarted = HttpContext.Current.Items.Contains("wei.MiniProfilerStarted") &&
            //     Convert.ToBoolean(HttpContext.Current.Items["wei.MiniProfilerStarted"]);
            //if (miniProfilerStarted)
            //{
            //    MiniProfiler.Stop();
            //}
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            //log error
            LogException(exception);
        }

        protected void LogException(Exception exc)
        {
            if (exc == null)
                return;

            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //ignore 404 HTTP errors
            var httpException = exc as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                return;

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
        }

        protected void SetWorkingCulture()
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            //ignore static resources
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            if (webHelper.IsStaticResource(this.Request))
                return;

            //keep alive page requested (we ignore it to prevent creation of guest customer records)
            string keepAliveUrl = string.Format("{0}keepalive/index", webHelper.GetStoreLocation());
            if (webHelper.GetThisPageUrl(false).StartsWith(keepAliveUrl, StringComparison.InvariantCultureIgnoreCase))
                return;
            CommonHelper.SetTelerikCulture();
        }

    }
}