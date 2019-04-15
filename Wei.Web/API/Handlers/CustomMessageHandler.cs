using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Wei.Core;
using Wei.Core.Domain.Questions;
using Wei.Core.Domain.Users;
using Wei.Services.Custom;
using Wei.Services.Logging;
using Wei.Services.Questions;
using Wei.Services.Users;

namespace Wei.Web.API.Handlers
{
    public partial class CustomMessageHandler: MessageHandler<CustomMessageContent>
    {
        private readonly IUserService _userService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IQuestionBankService _questionBankService;
        private readonly ILogger _logger;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        public static int REQUESTNO;

        //public CustomMessageHandler(Stream inputStream, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
        //    : base(inputStream, postModel, maxRecordCount, developerInfo)
        //{
        //    System.Threading.Interlocked.Increment(ref REQUESTNO);

        //    //_logger = EngineContext.Current.Resolve<ILogger>();
        //    //_userService = EngineContext.Current.Resolve<IUserService>();
        //    //_userAnswerService = EngineContext.Current.Resolve<IUserAnswerService>();
        //    //_questionBankService = EngineContext.Current.Resolve<IQuestionBankService>();
        //    //_webHelper = EngineContext.Current.Resolve<IWebHelper>();
        //    //_httpContext = EngineContext.Current.Resolve<HttpContextBase>();
        //}

        public CustomMessageHandler(XDocument requestDocument, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(requestDocument, postModel, maxRecordCount, developerInfo)
        {
            System.Threading.Interlocked.Increment(ref REQUESTNO);

            //_logger = EngineContext.Current.Resolve<ILogger>();
            //_userService = EngineContext.Current.Resolve<IUserService>();
            //_userAnswerService = EngineContext.Current.Resolve<IUserAnswerService>();
            //_questionBankService = EngineContext.Current.Resolve<IQuestionBankService>();
            //_webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //_httpContext = EngineContext.Current.Resolve<HttpContextBase>();
        }

        public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(requestMessageBase, postModel, maxRecordCount, developerInfo)
        {
            System.Threading.Interlocked.Increment(ref REQUESTNO);

            //_logger = EngineContext.Current.Resolve<ILogger>();
            //_userService = EngineContext.Current.Resolve<IUserService>();
            //_userAnswerService = EngineContext.Current.Resolve<IUserAnswerService>();
            //_questionBankService = EngineContext.Current.Resolve<IQuestionBankService>();
            //_webHelper = EngineContext.Current.Resolve<IWebHelper>();
            //_httpContext = EngineContext.Current.Resolve<HttpContextBase>();
        }

        public CustomMessageHandler(Stream inputStream, PostModel postModel
            , IUserService userService
            , IUserAnswerService userAnswerService
            , IQuestionBankService questionBankService
            , IWebHelper webHelper
            , HttpContextBase httpContext
            //, IWorkContext workContext
            , ILogger logger)
            : base(inputStream, postModel, 0, null)
        {
            System.Threading.Interlocked.Increment(ref REQUESTNO);

            _logger = logger;
            _userService = userService;
            _userAnswerService = userAnswerService;
            _questionBankService = questionBankService;
            _webHelper = webHelper;
            _httpContext = httpContext;
        }

        /// <summary>
        /// 自定义的消息回复
        /// </summary>
        /// <param name="content"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private IResponseMessageBase CustomResponse(string content, Wei.Core.Domain.Users.User user, string voicepath = null)
        {
            this._logger.Information("CustomResponse");
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            
            if (user == null || user.Subscribe == 0)
            {
                responseMessage.Content = "请先关注公众号！";
                return responseMessage;
            }
            // 获取当前答题信息
            var uanswer = this._userAnswerService.GetDoingQuestionAnswer(user.Id);
            if (uanswer == null)
            {
                // 获取有效的题卷启动Keys
                var defQuestionList = this._questionBankService.KeyWordQuestionBank();
                if (defQuestionList.ContainsKey(content))
                {
                    QuestionBank questionBank = defQuestionList[content];
                    // 判断用户是否答过当前题卷
                    if (this._userAnswerService.IsAnswered(questionBank.Id, user))
                    {
                        responseMessage.Content = "已答过该题卷，不能重复答题";
                    }
                    else
                    {
                        // 判断用户信息是否录入
                        if (user.Status == 0)
                        {
                            var token = Guid.NewGuid();
                            //_httpContext.Session.Add(token.ToString("N"), user);
                            this._webHelper.SetSessionObject<User>("tokenuser", token.ToString("N"), user);
                            string url = System.Configuration.ConfigurationManager.AppSettings["WebDomain"] + "/user/userinfo?tokens=" + token.ToString("N");
                            responseMessage.Content = $"请先录入个人信息：{url}";
                        }
                        else
                        {
                            // 开始执行答题
                            var question = this._userAnswerService.BeginQuestion(user.Id, questionBank.Id);
                            string qtext = question.Text;
                            if(question.AnswerType == AnswerType.SingleCheck || question.AnswerType == AnswerType.MultiCheck)
                            {
                                qtext += (Environment.NewLine + string.Join(Environment.NewLine, question.QuestionItemList.Select(x=>x.Text).ToArray())); 
                            }
                            var maxq = questionBank.QuestionList.Max(x => x.Sort);
                            responseMessage.Content = string.Format("【{0}/{1}】{2}", question.Sort, maxq, question.Text);
                        }
                    }
                }
                //string tmpContent = content.ToLower();
                //foreach (var key in defQuestionList.Keys)
                //{
                //    if (tmpContent.IndexOf(key.ToLower()) != -1)
                //    {
                //        questionBank = defQuestionList[key];
                //        break;
                //    }
                //}
                //if (questionBank != null)
                //{
                //    // 判断用户是否答过当前题卷
                //    if (this._userAnswerService.IsAnswered(questionBank.Id, user))
                //    {
                //        responseMessage.Content = "已答过该题卷，不能重复答题";
                //    }
                //    else
                //    {
                //        // 判断用户信息是否录入
                //        if (user.Status == 0)
                //        {
                //            var token = Guid.NewGuid();
                //            //_httpContext.Session.Add(token.ToString("N"), user);
                //            this._webHelper.SetSessionObject<User>("tokenuser", token.ToString("N"), user);
                //            string url = System.Configuration.ConfigurationManager.AppSettings["WebDomain"] + "/user/userinfo?tokens=" + token.ToString("N");
                //            responseMessage.Content = $"请先录入个人信息：{url}";
                //        }
                //        else
                //        {
                //            // 开始执行答题
                //            var question = this._userAnswerService.BeginQuestion(user.Id, questionBank.Id);
                //            var maxq = questionBank.QuestionList.Max(x => x.Sort);
                //            responseMessage.Content = string.Format("【{0}/{1}】{2}", question.Sort, maxq, question.Text);
                //        }
                //    }
                //    //// 开始前判断是否有必须录入的用户信息
                //    //// 任意个属性没有值，用户信息录入
                //    //if(questionBank.UserAttributeList.Any(x=> {
                //    //    var userattr = user.UserAttributeList.FirstOrDefault(ua => ua.UserAttributeId == x.Id);
                //    //    return userattr == null || string.IsNullOrEmpty(userattr.Value);
                //    //}))
                //    //{

                //    //}
                //}
                else
                {
                    responseMessage.Content = "我是复读机!..." + content;
                }
            }
            else
            {
                // 没有作答
                if (string.IsNullOrEmpty(content))
                {
                    responseMessage.Content = "无法检测您在说什么，请确认是否有作答或者语言清晰！";
                }
                else if (string.Equals("放弃答题", content))
                {
                    if(uanswer.UserAnswerStatus == Core.Domain.Custom.UserAnswerStatus.挂起)
                    {
                        uanswer.CompletedTime = DateTime.Now;
                        uanswer.UserAnswerStatus = Core.Domain.Custom.UserAnswerStatus.已作废;
                        this._userAnswerService.Save(uanswer);
                        responseMessage.Content = "谢谢您的参与！";
                    }
                    else
                    {
                        uanswer.UserAnswerStatus = Core.Domain.Custom.UserAnswerStatus.挂起;
                        this._userAnswerService.Save(uanswer);
                        responseMessage.Content = "是否确定放弃本次答题（本次答题数据将作废）？";
                    }
                }
                else
                {
                    if(uanswer.UserAnswerStatus == Core.Domain.Custom.UserAnswerStatus.挂起)
                    {
                        if(string.Equals(content, "是") || string.Equals(content, "是的") || string.Equals(content, "确定")
                            || string.Equals(content, "放弃") || string.Equals(content, "放弃答题"))
                        {
                            uanswer.CompletedTime = DateTime.Now;
                            uanswer.UserAnswerStatus = Core.Domain.Custom.UserAnswerStatus.已作废;
                            this._userAnswerService.Save(uanswer);
                        }
                        else
                        {
                            uanswer.UserAnswerStatus = Core.Domain.Custom.UserAnswerStatus.答题中;
                            this._userAnswerService.Save(uanswer);

                            // 开始答题了， 当前消息为答案
                            var question = this._userAnswerService.SaveAnswer(uanswer, content, user, voicepath);
                            var maxq = uanswer.QuestionBank.QuestionList.Max(x => x.Sort);
                            if (question == null)
                            {
                                responseMessage.Content = "您的回答似乎不符，请尝试重新作答！";
                            }
                            else
                            {
                                responseMessage.Content = string.Format("【{0}/{1}】{2}", question.Sort, maxq, question.Text);
                            }
                        }
                    }
                    else
                    {
                        // 开始答题了， 当前消息为答案
                        var question = this._userAnswerService.SaveAnswer(uanswer, content, user, voicepath);
                        var maxq = question.QuestionBank.QuestionList.Max(x => x.Sort);
                        if (question == null)
                        {
                            responseMessage.Content = "您的回答似乎不符，请尝试重新作答！";
                        }
                        else
                        {
                            responseMessage.Content = string.Format("【{0}/{1}】{2}", question.Sort, maxq, question.Text);
                        }
                    }
                }
            }
            return responseMessage;
        }

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


            responseMessage.Content = $"【{REQUESTNO}】您好，目前使用的微信公众号仍处于开发阶段，现已接入了【图灵机器人】，您可以尝试和他（她）交流。";
            return responseMessage;
            //var defaultResponseMessage = base.CreateResponseMessage<ResponseMessageText>();

            //if (requestMessage is RequestMessageText)
            //{
            //    var requestMsg = requestMessage as RequestMessageText;
            //    var requestHandler = requestMsg.StartHandler().Keyword("陈恩曦", () => {
            //        defaultResponseMessage.Content = "大笨蛋";
            //        return defaultResponseMessage;
            //    }).Keyword("陈京邑", () => {
            //        defaultResponseMessage.Content = "小金鱼";
            //        return defaultResponseMessage;
            //    }).Default(() => {
            //        defaultResponseMessage.Content = "啦啦啦 (～￣▽￣)～ ";
            //        return defaultResponseMessage;
            //    });
            //}
            //return defaultResponseMessage;
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
            //var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            //if (user == null || user.Subscribe == 0)
            //{
            //    responseMessage.Content = "请先关注公众号！";
            //    return responseMessage;
            //}
            //// 获取当前答题信息
            //var uanswer = this._userAnswerService.GetDoingQuestionAnswer(user.Id);
            //if(uanswer == null)
            //{
            //    //int questionbankid = this._webHelper.QueryString<int>("qbid");
            //    //if (questionbankid == 0)
            //    //    questionbankid = 1;
            //    var defQuestionList = this._questionBankService.KeyWordQuestionBank();
            //    QuestionBank questionBank = null;
            //    foreach(var key in defQuestionList.Keys)
            //    {
            //        if(requestMessage.Content.IndexOf(key) != -1)
            //        {
            //            questionBank = defQuestionList[key];
            //            //var question = this._userAnswerService.BeginQuestion(user.Id, defQuestionList[key].Id);
            //            //responseMessage.Content = question.Text;
            //            break;
            //        }
            //    }
            //    if(questionBank != null)
            //    {
            //        var question = this._userAnswerService.BeginQuestion(user.Id, questionBank.Id);
            //        responseMessage.Content = question.Text;
            //    }
            //    else
            //    {
            //        responseMessage.Content = "不清楚呢，请告诉你是要答题吗？";
            //    }
            //    // 没有开始答题
            //    //requestMessage.StartHandler().Keywords(defQuestionList.Keys.ToArray(), () => {
                    
            //    //        //var question = this._userAnswerService.BeginQuestion(user.Id, defQuestionList[]);
            //    //        responseMessage.Content = question.Text;
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    // 授权获取用户信息
            //    //    //    responseMessage.Content = $"{System.Configuration.ConfigurationManager.AppSettings["WebDomain"]}/authentication/useroauth";
            //    //    //}
            //    //    return responseMessage;
            //    //});
            //    //if (string.IsNullOrEmpty(responseMessage.Content))
            //    //{
            //    //    responseMessage.Content = "不清楚呢，请告诉你是要答题吗？";
            //    //}
            //    //{
            //    //    var question = this._userAnswerService.BeginQuestion(user.Id, 1);
            //    //    responseMessage.Content = question.Answer;
            //    //}
            //    //else
            //    //{
            //    //    responseMessage.Content = "不清楚呢，请告诉你是要答题吗？";
            //    //}
            //}
            //else
            //{
            //    // 开始答题了， 当前消息为答案
            //    var question = this._userAnswerService.SaveAnswer(uanswer, requestMessage.Content);
            //    if(question == null)
            //    {
            //        responseMessage.Content = "答卷完成，感谢您的参与！";
            //    }
            //    else
            //    {
            //        responseMessage.Content = question.Text;
            //    }
            //}
            
            ////requestMessage.StartHandler().Keyword("开始答题")
            ////var requestHandler = requestMessage.StartHandler().Keyword("陈恩曦", ()=> {
            ////    defaultResponseMessage.Content = "大笨蛋";
            ////    return defaultResponseMessage;
            ////}).Keyword("陈京邑", ()=> {
            ////    defaultResponseMessage.Content = "小金鱼";
            ////    return defaultResponseMessage;
            ////}).Default(()=> {
            ////    defaultResponseMessage.Content = "啦啦啦 (～￣▽￣)～ ";
            ////    return defaultResponseMessage;
            ////});
            ////var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            ////responseMessage.Content = $"【{REQUESTNO}】您刚才发送的文字信息是：{requestMessage.Content}。";  //\r\n用于换行，requestMessage.Content即用户发过来的文字内容
            //return responseMessage;
        }

        /// <summary>
        /// 图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = $"【{REQUESTNO}】您好，Image。";
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
            responseMessage.Content = $"【{REQUESTNO}】您好，File。";
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
            Stream stream = null;
            Senparc.Weixin.MP.AdvancedAPIs.MediaApi.Get(WXinConfig.WeixinAppId, requestMessage.MediaId, stream);
            if(stream != null)
            {
                byte[] bytes = bytes = new byte[stream.Length];
                
                stream.Read(bytes, 0, bytes.Length);
                voice = Convert.ToBase64String(bytes);
            }
            //// 获取当前答题信息
            //var uanswer = this._userAnswerService.GetDoingQuestionAnswer(user.Id);
            //if(uanswer != null)
            //{
            //    var uanswerdetail = uanswer.UserAnswerDetailList.LastOrDefault();
            //    var currq = this._questionBankService.GetQuestion(uanswer.QuestionBank_Id, uanswerdetail.QuestionNo);
            //    if(currq.AnswerType == AnswerType.SingleCheck || currq.AnswerType == AnswerType.MultiCheck)
            //    {
            //        var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            //        responseMessage.Content = "选择题暂不支持语音答题，请手写输入！";
            //        return responseMessage;
            //    }
            //}
            return CustomResponse(requestMessage.Recognition, user, voice);
        }

        /// <summary>
        /// 视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = $"【{REQUESTNO}】您好，Video。";
            return responseMessage;
        }
    }
}