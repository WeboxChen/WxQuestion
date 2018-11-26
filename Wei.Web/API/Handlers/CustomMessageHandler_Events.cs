using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Wei.Services.Users;

namespace Wei.Web.API.Handlers
{
    public partial class CustomMessageHandler
    {
        //private IUserService _userService;

        private string GetWelcomeInfo()
        {
            return "欢迎关注【Webox.Chen 微信订阅号】";
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            var user = _userService.GetUserById(requestMessage.FromUserName);
            if(user == null)
            {
                user = new Core.Domain.Users.User()
                {
                    OpenId = requestMessage.FromUserName,
                    CreateTime = DateTime.Now,
                    LastActivityTime = DateTime.Now,
                    Subscribe = 1
                };
                this._userService.InsertUser(user);
            }
            else
            {
                user.Subscribe = 1;
                this._userService.UpdateUser(user);
            }
            responseMessage.Content = GetWelcomeInfo();
            return responseMessage;
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var user = _userService.GetUserById(requestMessage.FromUserName);
            if(user  != null)
            {
                user.Subscribe = 0;
                this._userService.UpdateUser(user);
            }
            return base.OnEvent_UnsubscribeRequest(requestMessage);
        }
    }
}