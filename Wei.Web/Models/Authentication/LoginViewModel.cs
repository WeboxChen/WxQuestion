using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wei.Web.Models.Authentication
{
    public class LoginRequestViewModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}