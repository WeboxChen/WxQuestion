using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using Wei.Core;
using Wei.Core.Infrastructure;

namespace Wei.Web.Framework.Controllers
{
    /// <summary>
    /// 用户验证属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class UserApiAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly bool _dontValidate;


        public UserApiAuthorizeAttribute()
            : this(false)
        {
        }

        public UserApiAuthorizeAttribute(bool dontValidate)
        {
            this._dontValidate = dontValidate;
        }

        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (_dontValidate)
                return;

            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            var context = EngineContext.Current.Resolve<IWorkContext>();
            var user = context.CurrentUser;
            if (user == null)
                HandleUnauthorizedRequest(filterContext);
            //if(user.LoginTicket)

            // throw new InvalidOperationException("Your account is expired!");
            //var userService = EngineContext.Current.Resolve<IUserService>();
            //if (userService.CheckPermission() == null)
            //    throw new InvalidOperationException("Your account is expired!");

        }
    }
}
