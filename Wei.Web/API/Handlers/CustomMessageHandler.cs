using Senparc.NeuChar.App.AppStore;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Wei.Core;
using Wei.Core.Configuration;
using Wei.Core.Domain.Custom;
using Wei.Core.Domain.Questions;
using Wei.Core.Domain.Users;
using Wei.Core.Infrastructure;
using Wei.Services.Custom;
using Wei.Services.Logging;
using Wei.Services.Questions;
using Wei.Services.Sys;
using Wei.Services.Users;

namespace Wei.Web.API.Handlers
{
    public partial class CustomMessageHandler: MessageHandler<CustomMessageContent>
    {
        private readonly IUserService _userService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IQuestionBankService _questionBankService;
        private readonly IPropertyService _propertyService;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        //public static int REQUESTNO;

        public CustomMessageHandler(XDocument requestDocument, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(requestDocument, postModel, maxRecordCount, developerInfo)
        {
            //System.Threading.Interlocked.Increment(ref REQUESTNO);
        }

        public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(requestMessageBase, postModel, maxRecordCount, developerInfo)
        {
            //System.Threading.Interlocked.Increment(ref REQUESTNO);
        }

        public CustomMessageHandler(Stream inputStream, PostModel postModel
            , IUserService userService
            , IUserAnswerService userAnswerService
            , IQuestionBankService questionBankService
            , IPropertyService propertyService
            , IWebHelper webHelper
            , HttpContextBase httpContext
            , IWorkContext workContext
            , ILogger logger)
            : base(inputStream, postModel, 0, null)
        {
            //System.Threading.Interlocked.Increment(ref REQUESTNO);

            this._logger = logger;
            this._userService = userService;
            this._userAnswerService = userAnswerService;
            this._questionBankService = questionBankService;
            this._propertyService = propertyService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._workContext = workContext;
        }

        /// <summary>
        /// 自定义的消息回复
        /// </summary>
        /// <param name="content"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private IResponseMessageBase CustomResponse(string content, Wei.Core.Domain.Users.User user, MediaType mediaType = MediaType.None, string mediaContent = null)
        {
            //this._logger.Information("CustomMessageHandler.CustomResponse");
            this._workContext.CurrentUser = user;
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (user == null || user.Subscribe == 0)
            {
                responseMessage.Content = this._propertyService.GetValue("Unfollow");
                return responseMessage;
            }
            // 获取当前答题信息
            var uanswer = this._userAnswerService.GetDoingUserAnswer(user.Id);
            if (uanswer == null)
            {
                // 获取有效的题卷启动Keys
                var defQuestionList = this._questionBankService.KeyWordQuestionBank();
                if (defQuestionList.ContainsKey(content))
                {
                    QuestionBank questionBank = defQuestionList[content];
                    // 判断用户是否答过当前题卷
                    var qanswer = this._userAnswerService.GetUserAnswerByQBId(user.Id, questionBank.Id);
                    if(qanswer == null)
                    {
                        // 判断用户信息是否录入
                        if (user.Status == 0)
                        {
                            var token = Guid.NewGuid();
                            this._webHelper.SetSessionObject<User>("tokenuser", token.ToString("N"), user);
                            string url = System.Configuration.ConfigurationManager.AppSettings["WebDomain"] + "/user/userinfo?tokens=" + token.ToString("N");
                            responseMessage.Content = $"{this._propertyService.GetValue("UserInfoTips")}：{url}";
                        }
                        else
                        {
                            // 开始执行答题
                            var question = this._userAnswerService.BeginQuestion(user.Id, questionBank.Id);
                            //if (question != null && question.AnswerType == AnswerType.Tips)
                            //{
                            //    TipsQuestionTask(question.Id, user.OpenId);
                            //}
                            responseMessage.Content = question.QuestionText;
                            //this._logger.Warning($"text: {question.Text}, questiontext: {question.QuestionText}");
                        }
                    }
                    else
                    {
                        switch (qanswer.UserAnswerStatus)
                        {
                            case UserAnswerStatus.挂起:
                                // 开始执行答题
                                var question = this._userAnswerService.BeginQuestion(user, qanswer);
                                //if (question != null && question.AnswerType == AnswerType.Tips)
                                //{
                                //    TipsQuestionTask(question.Id, user.OpenId);
                                //}
                                responseMessage.Content = question.QuestionText;
                                break;
                            case UserAnswerStatus.答题完成:
                                responseMessage.Content = this._propertyService.GetValue("Answer.NotRepeat");
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    responseMessage.Content = "我是复读机!..." + content;
                }
            }
            else
            {
                // 没有作答
                if (mediaType == MediaType.Voice && string.IsNullOrEmpty(content))
                {
                    responseMessage.Content = this._propertyService.GetValue("Answer.Empty");
                }
                else if(this._propertyService.CheckCodeValue("Answer.Cancel", content)) // if (string.Equals("放弃答题", content))
                {
                    uanswer.UserAnswerStatus = Core.Domain.Custom.UserAnswerStatus.挂起;
                    this._userAnswerService.Save(uanswer);
                    responseMessage.Content = this._propertyService.GetValue("Answer.CancelTips");
                }
                else
                {
                    try
                    {
                        // 开始答题了， 当前消息为答案
                        var question = this._userAnswerService.SaveAnswer(uanswer, content, user, mediaType, mediaContent);
                        //if(question != null && question.AnswerType == AnswerType.Tips)
                        //{
                        //    TipsQuestionTask(question.Id, user.OpenId);
                        //}
                        if(question.Sort.ToDouble().ToString().IndexOf('.') != -1)
                        {
                            // 获取刚刚的答案
                            var uanswerdetail = uanswer.UserAnswerDetailList.FirstOrDefault(x => x.End == null); // uanswer.UserAnswerDetailList.Where(x=>x.End != null).OrderByDescending(x => x.Id).FirstOrDefault();
                            string qcode = null;
                            if (uanswerdetail != null)
                                qcode = uanswerdetail.QCode;
                            if(qcode  == null)
                                qcode = "";
                            responseMessage.Content = question.QuestionText.Replace("{CODE}", qcode);
                        }
                        else
                        {
                            responseMessage.Content = question.QuestionText;
                        }
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(ex.Message, ex);
                    }
                }
            }

            if (string.IsNullOrEmpty(responseMessage.Content))
                responseMessage.MsgType = Senparc.NeuChar.ResponseMsgType.NoResponse;
            return responseMessage;
        }
        
        //private static void TipsQuestionTask(int questionid, string openid)
        //{
        //    Task task = new Task(() =>
        //    {
        //        ILogger logger = EngineContext.Current.Resolve<ILogger>();
        //        logger.Information("TipsQuestionTask start");
        //        System.Threading.Thread.Sleep(500);
        //        IUserService uservice = EngineContext.Current.Resolve<IUserService>();
        //        IUserAnswerService uaService = EngineContext.Current.Resolve<IUserAnswerService>();
        //        IQuestionBankService qbService = EngineContext.Current.Resolve<IQuestionBankService>();
        //        IWorkContext workcontext = EngineContext.Current.Resolve<IWorkContext>();

        //        Question question = qbService.GetQuestionById(questionid);
        //        QuestionBank qbank = qbService.GetQuestionBankById(question.QuestionBank_Id);
        //        User user = uservice.GetUserById(openid);
        //        UserAnswer uanswer = uaService.GetDoingUserAnswer(user.Id);
        //        workcontext.CurrentUser = user;
        //        var nquesiont = uaService.SaveAnswer(uanswer, "", user);
        //        if (nquesiont.AnswerType == AnswerType.Tips)
        //        {
        //            TipsQuestionTask(nquesiont.Id, openid);
        //        }
        //        logger.Information("TipsQuestionTask end");
        //    });
        //    task.Start();
        //}

        /// <summary>
        /// 默认
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            var user = this._userService.GetUserById(requestMessage.FromUserName);
            if(user == null || user.Subscribe == 0)
            {
                responseMessage.Content = "请先关注公众号！";
                return responseMessage;
            }


            //responseMessage.Content = $"【{REQUESTNO}】您好，目前使用的微信公众号仍处于开发阶段,DefaultMessage。";
            return responseMessage;
        }

        /// <summary>
        /// 文本请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var user = this._userService.GetUserById(requestMessage.FromUserName);
            return CustomResponse(requestMessage.Content, user);
        }

        /// <summary>
        /// 图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var user = this._userService.GetUserById(requestMessage.FromUserName);
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            if (user == null || user.Subscribe == 0)
            {
                responseMessage.Content = "请先关注公众号！";
                return responseMessage;
            }
            // 下载图片
            string image = null;
            Stream stream = null;
            try
            {
                string filedir = Path.Combine(WXinConfig.MediaDir, "image\\");
                if (!Directory.Exists(filedir))
                    Directory.CreateDirectory(filedir);
                string filepath = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.Get(WXinConfig.WeixinAppId, requestMessage.MediaId, filedir);
                stream = File.OpenRead(filepath);
            }
            catch (Exception ex)
            {
                this._logger.Information(string.Format("WXinConfig.WeixinAppId:{0}; requestMessage.MediaId:{1}", WXinConfig.WeixinAppId, requestMessage.MediaId));
                this._logger.Error(ex.Message, ex);
            }
            if (stream != null)
            {
                byte[] bytes = bytes = new byte[stream.Length];

                stream.Read(bytes, 0, bytes.Length);
                image = Convert.ToBase64String(bytes);
                stream.Close();
            }
            
            // 获取当前答题信息
            var uanswer = this._userAnswerService.GetDoingUserAnswer(user.Id);
            if (uanswer == null)
            {
                responseMessage.Content = "";
            }
            else
            {
                if (uanswer.UserAnswerStatus == Core.Domain.Custom.UserAnswerStatus.挂起)
                {
 
                    responseMessage.Content = "当前答题已暂停，请确认是否继续答题【继续（Y）/放弃（N）】！";
                }
                else
                {
                    // 开始答题了， 当前消息为答案
                    var question = this._userAnswerService.SaveAnswerImage(uanswer, user, image);

                    responseMessage.Content = question.QuestionText;
                }
            }
            return responseMessage;
        }

        /// <summary>
        /// 文件请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnFileRequest(RequestMessageFile requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            //responseMessage.Content = $"【{REQUESTNO}】您好，暂不支持文件上传。";
            return responseMessage;
        }

        /// <summary>
        /// 语言请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var user = this._userService.GetUserById(requestMessage.FromUserName);
            string voice = null;
            using(Stream stream = new MemoryStream())
            {
                try
                {
                    //string filedir = Path.Combine(WXinConfig.MediaDir, "voice\\");
                    //if (!Directory.Exists(filedir))
                    //    Directory.CreateDirectory(filedir);
                    //string filepath = Senparc.Weixin.MP.AdvancedAPIs.MediaApi.Get(WXinConfig.WeixinAppId, requestMessage.MediaId, filedir);
                    Senparc.Weixin.MP.AdvancedAPIs.MediaApi.Get(WXinConfig.WeixinAppId, requestMessage.MediaId, stream);
                    //if(result.errcode != Senparc.Weixin.ReturnCode.请求成功)
                    //    this._logger.Information(string.Format("WXinConfig.WeixinAppId:{0}; requestMessage.MediaId:{1}; errormsg: {2}"
                    //        , WXinConfig.WeixinAppId, requestMessage.MediaId, result.errmsg));
                    //stream = File.OpenRead(filepath);

                }
                catch (Exception ex)
                {
                    this._logger.Information(string.Format("WXinConfig.WeixinAppId:{0}; requestMessage.MediaId:{1}", WXinConfig.WeixinAppId, requestMessage.MediaId));
                    this._logger.Error(ex.Message, ex);
                }
                if (stream != null)
                {
                    byte[] bytes = bytes = new byte[stream.Length];

                    stream.Position = 0;
                    stream.Read(bytes, 0, bytes.Length);
                    voice = Convert.ToBase64String(bytes);
                    stream.Close();
                }
            }

            return CustomResponse(requestMessage.Recognition.TrimEnd('。'), user, MediaType.Voice, voice);
        }

        /// <summary>
        /// 视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            //responseMessage.Content = $"【{REQUESTNO}】您好，暂不支持视频功能。";
            return responseMessage;
        }
    }
}