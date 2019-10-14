using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Web;
using System.Web.Mvc;
using Wei.Core;
using Wei.Core.Configuration;
using Wei.Services.Custom;
using Wei.Services.Logging;
using Wei.Services.Questions;
using Wei.Services.Sys;
using Wei.Services.Users;
using Wei.Web.API.Handlers;

namespace Wei.Web.Controllers
{
    public class WeiXinController : Controller
    {
        #region fields 
        private readonly IUserService _userService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IQuestionBankService _questionBankService;
        private readonly IPropertyService _propertyService;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;

        #endregion

        public WeiXinController(IUserService userService
            , IUserAnswerService userAnswerService
            , IQuestionBankService questionBankService
            , IPropertyService propertyService
            , IWebHelper webHelper
            , HttpContextBase httpContext
            , IWorkContext workContext
            , ILogger logger)
        {
            this._logger = logger;
            this._userService = userService;
            this._userAnswerService = userAnswerService;
            this._questionBankService = questionBankService;
            this._propertyService = propertyService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._workContext = workContext;
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
            try
            {
                //校验签名
                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WXinConfig.WeixinToken))
                {
                    this._logger.Information($"参数错误！Signature:{postModel.Signature},WeixinToken:{WXinConfig.WeixinToken}");
                    return new WeixinResult("参数错误！");
                }

                postModel.AppId = WXinConfig.WeixinAppId;
                postModel.EncodingAESKey = WXinConfig.WeixinEncodingAESKey;
                postModel.Token = WXinConfig.WeixinToken;

                //接收消息，自定义 MessageHandler，对微信请求进行处理
                var messageHandler = new CustomMessageHandler(Request.InputStream, postModel, _userService, _userAnswerService
                    , _questionBankService, _propertyService, _webHelper, _httpContext, _workContext, _logger);

                #region 设置消息去重

                /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                 * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能

                #endregion
                //_logger.Warning(messageHandler.RequestMessage.MsgType.ToString());

                //执行微信处理过程
                messageHandler.Execute();
                return new FixWeixinBugWeixinResult(messageHandler);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
                return null;
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
        }

    }
}