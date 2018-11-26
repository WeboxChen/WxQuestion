using System.Web.Mvc;
using System.Web.Routing;

namespace Wei.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "WeiXin", action = "Index" },
                new[] { "Wei.Web.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "authentication/login"
            );
        }
    }
}
