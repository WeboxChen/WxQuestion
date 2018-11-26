using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wei.Web.Framework.Controllers;

namespace Wei.Web.Controllers
{
    public class SettingController : BaseController
    {



        // GET: Setting
        public ActionResult Index()
        {
            return View();
        }
    }
}