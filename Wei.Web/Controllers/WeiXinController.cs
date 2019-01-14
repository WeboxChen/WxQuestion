using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
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
            //throw new System.Exception("不告诉你哪里有问题");
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
            try
            {
                //执行微信处理过程
                messageHandler.Execute();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
            //try
            //{
            //    if (messageHandler.ResponseMessage is ResponseMessageText)
            //    {
            //        // 判断内容是否为跳转
            //        var resp = messageHandler.ResponseMessage as ResponseMessageText;
            //        if (resp != null && resp.Content != null && resp.Content.IndexOf("redirect=") == 0)
            //        {
            //            // 保留请求信息，以后跳转使用
            //            Session[resp.ToUserName] = postModel;
            //            Session.Timeout = 30;

            //            string redirectStr = resp.Content.Split('=')[1];
            //            string[] controlAndAction = redirectStr.Split('/');
            //            _logger.Information(string.Join(",", controlAndAction));
            //            if (controlAndAction.Length == 2)
            //                return RedirectToAction(controlAndAction[1], controlAndAction[0]);
            //            return RedirectToAction(controlAndAction[0]);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.Message, ex);
            //}
            //返回处理结果
            return new FixWeixinBugWeixinResult(messageHandler);
        }

        [HttpGet]
        public ActionResult Error()
        {
            var error = Server.GetLastError();
            _logger.Error(error.Message, error);
            return View();
        }
    }
}