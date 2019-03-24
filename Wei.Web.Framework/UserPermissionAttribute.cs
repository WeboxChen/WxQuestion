using System;
using System.Web.Mvc;
using Wei.Core;
using Wei.Core.Data;
using Wei.Core.Infrastructure;
using Wei.Services.Users;

namespace Wei.Web.Framework
{
    public class UserPermissionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!DataSettingsHelper.DatabaseIsInstalled())
                return;

            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;
            
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var userService = EngineContext.Current.Resolve<IUserService>();


            var user = workContext.CurrentUser;
            if (user == null)
            {
                // 验证用户
                var permission = userService.CheckPermission();
                if(permission == null)
                {
                    filterContext.HttpContext.Response.RedirectToRoute("Login");
                    return;
                }
                user = permission.User;
            }

            //update last activity date
            if (user.LastActivityTime.AddMinutes(1.0) < DateTime.UtcNow)
            {
                user.LastActivityTime = DateTime.UtcNow;
                userService.UpdateUser(user);
            }
        }
    }
}
