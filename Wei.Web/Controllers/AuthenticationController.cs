using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wei.Core;
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
        private readonly IWebHelper _webHelper;
        #endregion

        #region cotr
        public AuthenticationController(IUserRegistrationService userRegistrationService
            , IAuthenticationService authenticationService
            , IUserService userService
            , IWebHelper webHelper)
        {
            this._userRegistrationService = userRegistrationService;
            this._authenticationService = authenticationService;
            this._userService = userService;
            this._webHelper = webHelper;
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
                    Remark = user.Remark,
                    Sex = user.Sex
                };
                return Json(ResponseMessage.Success("", usermodel));
            }
            else
            {
                return Json(ResponseMessage.Error(loginResult.ToString()));
            }
        }
    }
}