using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wei.Web.Framework.Controllers;

namespace Wei.Web.Controllers
{
    public class WeiXinController : Controller
    {
        string token = "weixin";

        [HttpGet]
        // GET: WeiXin
        public ActionResult Index(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, token))
            {
                return Content(echostr);
            }
            else
            {
                return Content(string.Format("failed:{0},{1}.如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。"
                    , postModel.Signature, Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, token)));
            }
        }
    }
}