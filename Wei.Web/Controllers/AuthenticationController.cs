using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wei.Core;
using Wei.Core.Configuration;
using Wei.Services.Logging;
using Wei.Services.Users;
using Wei.Web.Framework.Controllers;
using Wei.Web.Framework.ExtJs;
using Wei.Web.Models.Authentication;
using Wei.Web.Models.Users;

namespace Wei.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        #region fields
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly ILogger _logger;
        #endregion

        #region cotr
        public AuthenticationController(IUserRegistrationService userRegistrationService
            , IAuthenticationService authenticationService
            , IUserService userService
            , IWorkContext workContext
            , IWebHelper webHelper
            , HttpContextBase httpContext
            , ILogger logger)
        {
            this._userRegistrationService = userRegistrationService;
            this._authenticationService = authenticationService;
            this._userService = userService;
            this._workContext = workContext;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._logger = logger;
        }
        #endregion

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [JsonCallback]
        public ActionResult Login(LoginRequestViewModel model)
        {
            var loginResult = _userRegistrationService.ValidateUser(model.LoginName, model.Password);
            if(loginResult == Core.Domain.Users.UserLoginResults.Successful)
            {
                var user = this._userService.GetUserByLoginName(model.LoginName);
                _authenticationService.SignIn(user, model.RememberMe);

                var usermodel = new UserViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsAdmin = user.IsAdmin,
                    Language = user.Language,
                    LoginName = user.LoginName,
                    Phone = user.Phone,
                    OpenId = user.OpenId,
                    QQ = user.QQ,
                    Sex = user.Sex,
                    Address = user.Address,
                    Married = user.Married == null ? false : user.Married.Value,
                    Education = user.Education,
                    IdentityCard = user.IdentityCard
                };
                return Json(ResponseMessageExt.Success("", usermodel));
            }
            else
            {
                return Json(ResponseMessageExt.Failed(loginResult.ToString()));
            }
        }


        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [JsonCallback]
        [HttpPost]
        public ActionResult Logout()
        {
            this._authenticationService.SignOut();
            HttpCookie tokenCookie = this._httpContext.Response.Cookies.Get("Token");
            if (tokenCookie != null)
                tokenCookie.Expires = DateTime.Now.AddDays(-1);
            HttpCookie userCookie = this._httpContext.Response.Cookies.Get("Wei.user");
            if (userCookie != null)
                userCookie.Expires = DateTime.Now.AddDays(-1);
            return Json(ResponseMessageExt.Success());
        }

        /// <summary>
        /// 验证登录状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckLogin()
        {
            if (this._workContext.CurrentUser == null)
                return Json(ResponseMessageExt.Failed());
            return Json(ResponseMessageExt.Success());
        }

        public ActionResult UserOAuth()
        {
            this._logger.Debug("UserOAuth");
            string redirect_uri = $"{System.Configuration.ConfigurationManager.AppSettings["WebDomain"]}/authentication/UserOAuthCallBack";
            this._logger.Debug(redirect_uri);
            string state = "wx" + DateTime.Now.Millisecond;
            Session["state"] = state;//一旦比较完之后需要清空必须清空
            string redirect = OAuthApi.GetAuthorizeUrl(WXinConfig.WeixinAppId, redirect_uri, state, Senparc.Weixin.MP.OAuthScope.snsapi_base);

            return Redirect(redirect);
        }

        
        public ActionResult UserOAuthCallBack(string code, string state)
        {
            if (Session["state"].ToString() != state)
            {
                Session["state"] = null;
                return Content("请重新进入");
            }
            Session["state"] = null;
            if (string.IsNullOrEmpty(code))
                return RedirectToAction("UserOAuth");
            var accessToken = OAuthApi.GetAccessToken(WXinConfig.WeixinAppId, WXinConfig.WeixinAppSecret, code);
            if(accessToken.errcode != Senparc.Weixin.ReturnCode.请求成功)
            {
                return Content(accessToken.errcode.ToString());
            }
            try
            {
                //var wxuser = OAuthApi.GetUserInfo(accessToken.access_token, accessToken.openid);
                //var user = this._userService.GetUserById(wxuser.openid);
                //user.NickName = wxuser.nickname;
                //user.City = wxuser.city;
                //user.Country = wxuser.country;
                //user.HeadImgUrl = wxuser.headimgurl;
                //user.Province = wxuser.province;
                //user.Sex = wxuser.sex;
                //user.UnionId = wxuser.unionid;
                //this._logger.Debug(JsonConvert.SerializeObject(wxuser), null, user);
                //this._userService.UpdateUser(user);
                return Content("欢迎！");
                //if(Session[user.OpenId] == null)
                //{
                //    return Content("操作超时，请重新来过！");
                //}
                //return RedirectToAction("Index", "WeiXin",  new { postModel = Session[user.OpenId] });
                //var user = this.
                //return Redirect(returnUrl);
            }
            catch
            {
                //如果没有获取到用户的信息，则需要重新进去授权界面

                string redirect_uri = $"{System.Configuration.ConfigurationManager.AppSettings["WebDomain"]}{Url.Action("UserOAuthCallBack")}";
                string state1 = "wx" + DateTime.Now.Millisecond;
                Session["state"] = state1;//一旦比较完之后需要清空必须清空
                string redirect = OAuthApi.GetAuthorizeUrl(WXinConfig.WeixinAppId, redirect_uri, state1, Senparc.Weixin.MP.OAuthScope.snsapi_base);
                return Redirect(redirect);
            }
        }
    }
}