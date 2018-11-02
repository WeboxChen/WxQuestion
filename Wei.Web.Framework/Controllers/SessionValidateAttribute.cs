using System;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Wei.Core;
using Wei.Core.Infrastructure;
using Wei.Services.Users;

namespace Wei.Web.Framework.Controllers
{
    public class SessionValidateAttribute : ActionFilterAttribute
    {
        public const string SessionKeyName = "SessionKey";
        public const string LogonName = "LogonName";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var qs = HttpUtility.ParseQueryString(filterContext.Request.RequestUri.Query);
            string sessionKey = qs[SessionKeyName];

            if (string.IsNullOrEmpty(sessionKey))
            {
                throw new WeiException("Invalid Session.", "InvalidSession");
            }
            
            IAuthenticationService authenticationService = EngineContext.Current.Resolve<IAuthenticationService>();
            IUserService userService = EngineContext.Current.Resolve<IUserService>();

            //validate user session
            var authenticationUser = authenticationService.GetAuthenticatedUser(); //  .GetUserDevice(sessionKey);
            var user = userService.GetUserById(authenticationUser.Id);
            

            if (user == null)
            {
                throw new WeiException("sessionKey not found", "RequireParameter_sessionKey");
            }
            else
            {
                //todo: 加Session是否过期的判断
                if (user.LastActivityTime.AddMinutes(30) < DateTime.Now)
                    throw new WeiException("session expired", "SessionTimeOut");

                filterContext.ControllerContext.RouteData.Values[LogonName] = user;
                
                user.LastActivityTime = DateTime.UtcNow;
                userService.UpdateUser(user);
            }
        }
    }
}
