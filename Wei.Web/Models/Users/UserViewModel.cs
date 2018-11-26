using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wei.Web.Models.Users
{
    public class UserViewModel
    {
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get { return FirstName + LastName; } }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string QQ { get; set; }
        public int IsAdmin { get; set; }
        public string OpenId { get; set; }
        public int Sex { get; set; }
        public string Language { get; set; }
        public string Remark { get; set; }
    }
}