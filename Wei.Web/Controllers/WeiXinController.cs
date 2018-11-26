using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System.Web.Mvc;
using Wei.Services.Logging;
using Wei.Web.API.Handlers;

namespace Wei.Web.Controllers
{
    public class WeiXinController : Controller
    {
        #region fields 
        private readonly ILogger _logger;
        
        #endregion

        public WeiXinController(ILogger logger)
        {
            this._logger = logger;
        }

        [HttpGet]
        // GET: WeiXin
        public ActionResult Index(PostModel postModel, string echostr)
        {
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WXinConfig.WeixinToken))
            {
                return Content(echostr);
            }
            else
            {
                return Content(string.Format("failed:{0},{1}.如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。"
                    , postModel.Signature, Senparc.Weixin.MP.CheckSignature.GetSignature(postModel.Timestamp, postModel.Nonce, WXinConfig.WeixinToken)));
            }
        }


        /// <summary>
        /// 处理用户发送消息后
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(PostModel postModel)
        {
            //校验签名
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WXinConfig.WeixinToken))
                return new WeixinResult("参数错误！");

            postModel.AppId = WXinConfig.WeixinAppId;
            postModel.EncodingAESKey = WXinConfig.WeixinEncodingAESKey;
            postModel.Token = WXinConfig.WeixinToken;

            //接收消息，自定义 MessageHandler，对微信请求进行处理
            var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);
            //_logger.Warning(messageHandler.RequestMessage.MsgType.ToString());
            //执行微信处理过程
            messageHandler.Execute();
            //返回处理结果
            return new FixWeixinBugWeixinResult(messageHandler);
        }

    }
}