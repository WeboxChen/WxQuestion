using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Entities.Request;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System.IO;
using System.Xml.Linq;
using Wei.Core.Infrastructure;
using Wei.Services.Custom;
using Wei.Services.Logging;
using Wei.Services.Users;

namespace Wei.Web.API.Handlers
{
    public partial class CustomMessageHandler: MessageHandler<CustomMessageContent>
    {
        private IUserService _userService;
        private IUserAnswerService _userAnswerService;
        private ILogger _logger;
        public static int REQUESTNO;

        public CustomMessageHandler(Stream inputStream, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(inputStream, postModel, maxRecordCount, developerInfo)
        {
            System.Threading.Interlocked.Increment(ref REQUESTNO);

            _logger = EngineContext.Current.Resolve<ILogger>();
            _userService = EngineContext.Current.Resolve<IUserService>();
            _userAnswerService = EngineContext.Current.Resolve<IUserAnswerService>();
        }

        public CustomMessageHandler(XDocument requestDocument, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(requestDocument, postModel, maxRecordCount, developerInfo)
        {
            System.Threading.Interlocked.Increment(ref REQUESTNO);

            _userService = EngineContext.Current.Resolve<IUserService>();
            _userAnswerService = EngineContext.Current.Resolve<IUserAnswerService>();
        }

        public CustomMessageHandler(RequestMessageBase requestMessageBase, PostModel postModel = null, int maxRecordCount = 0, DeveloperInfo developerInfo = null) 
            : base(requestMessageBase, postModel, maxRecordCount, developerInfo)
        {
            System.Threading.Interlocked.Increment(ref REQUESTNO);

            _userService = EngineContext.Current.Resolve<IUserService>();
            _userAnswerService = EngineContext.Current.Resolve<IUserAnswerService>();
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
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();

            var user = this._userService.GetUserById(requestMessage.FromUserName);
            if (user == null || user.Subscribe == 0)
            {
                responseMessage.Content = "请先关注公众号！";
                return responseMessage;
            }
            // 获取当前答题信息
            var uanswer = this._userAnswerService.GetDoingQuestionAnswer(user.Id);
            if(uanswer == null)
            {
                // 没有开始答题
                requestMessage.StartHandler().Keywords(new[] { "答题", "出题" }, () => {
                    var question = this._userAnswerService.BeginQuestion(user.Id, 1);
                    responseMessage.Content = question.Text;
                    return responseMessage;
                });
                if(string.IsNullOrEmpty( responseMessage.Content))
                    responseMessage.Content = "不清楚呢，请告诉你是要答题吗？";
                //{
                //    var question = this._userAnswerService.BeginQuestion(user.Id, 1);
                //    responseMessage.Content = question.Answer;
                //}
                //else
                //{
                //    responseMessage.Content = "不清楚呢，请告诉你是要答题吗？";
                //}
            }
            else
            {
                // 开始答题了， 当前消息为答案
                var question = this._userAnswerService.SaveAnswer(uanswer, requestMessage.Content);
                if(question == null)
                {
                    responseMessage.Content = "答卷完成，感谢您的参与！";
                }
                else
                {
                    responseMessage.Content = question.Text;
                }
            }
            
            //requestMessage.StartHandler().Keyword("开始答题")
            //var requestHandler = requestMessage.StartHandler().Keyword("陈恩曦", ()=> {
            //    defaultResponseMessage.Content = "大笨蛋";
            //    return defaultResponseMessage;
            //}).Keyword("陈京邑", ()=> {
            //    defaultResponseMessage.Content = "小金鱼";
            //    return defaultResponseMessage;
            //}).Default(()=> {
            //    defaultResponseMessage.Content = "啦啦啦 (～￣▽￣)～ ";
            //    return defaultResponseMessage;
            //});
            //var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            //responseMessage.Content = $"【{REQUESTNO}】您刚才发送的文字信息是：{requestMessage.Content}。";  //\r\n用于换行，requestMessage.Content即用户发过来的文字内容
            return responseMessage;
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
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>(); //ResponseMessageText也可以是News等其他类型
            responseMessage.Content = $"【{REQUESTNO}】您好，Voice。Message: {requestMessage.Recognition}";
            return responseMessage;
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