using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wei.Core;
using Wei.Core.Domain.Users;
using Wei.Services.Users;
using Wei.Web.Framework.Controllers;
using Wei.Web.Models.Users;

namespace Wei.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public UserController(IUserService userService
            , IWebHelper webHelper
            , IWorkContext workContext)
        {
            this._userService = userService;
            this._webHelper = webHelper;
            this._workContext = workContext;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserInfo()
        {

            //var token = this._httpContext.Request["tokens"];
            //if (string.IsNullOrEmpty(token))
            //{
            //    return RedirectToAction("NonUserSession");
            //}
            //var user = this._webHelper.GetSessionObject<User>("tokenuser", token);
            //if (user == null)
            //{
            //    return RedirectToAction("NonUserSession");
            //}
            //_workContext.CurrentUser = user;
            var user = _workContext.CurrentUser;
            if(user == null )
                return RedirectToAction("NonUserSession");

            UserViewModel usermodel = new UserViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                LoginName = user.LoginName,
                Address = user.Address,
                Education = user.Education,
                Email = user.Email,
                Sex = user.Sex,
                IdentityCard = user.IdentityCard,
                Married = user.Married == null ? false : user.Married.Value,
                QQ = user.QQ,
                Phone = user.Phone,
                Language = user.Language,
                OpenId = user.OpenId,
                Birthdate = user.Birthdate == null? new DateTime(2000, 1,1) : user.Birthdate.Value
            };
            
            return View(usermodel);
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult UserInfo(UserViewModel uimodel, FormCollection form)
        {
            if (!ModelState.IsValid)
            {
                return View(uimodel);
            }
            var usertoen = _workContext.CurrentUser;
            var user = this._userService.GetUserById(usertoen.Id);
            user.FirstName = uimodel.FirstName;
            user.LastName = uimodel.LastName;
            user.Address = uimodel.Address;
            user.Education = uimodel.Education;
            user.Email = uimodel.Email;
            user.Sex = uimodel.Sex;
            user.IdentityCard = uimodel.IdentityCard;
            user.QQ = uimodel.QQ;
            user.Phone = uimodel.Phone;
            user.Married = uimodel.Married;
            user.Status = 1;
            user.Birthdate = uimodel.Birthdate;

            this._userService.UpdateUser(user);
            return Redirect("~/closepager.html");
        }

        [HttpGet]
        public ActionResult NonUserSession()
        {
            return View();
        }
    }
}