using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.Entities;
using Wei.Core.Configuration;
using Wei.Web.ViewServices.Users;

namespace Wei.Web.API.Handlers
{
    public partial class CustomMessageHandler
    {
        //private IUserService _userService;

        private string GetWelcomeInfo(string username)
        {
            return $"【{username}】欢迎关注";
        }

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            UserViewService uvservice = new UserViewService();
            var user = uvservice.SaveUser(WXinConfig.WeixinAppId, requestMessage.FromUserName);
            responseMessage.Content = GetWelcomeInfo(user.NickName);
            //var user = _userService.GetUserById(requestMessage.FromUserName);
            //if(user == null)
            //{
            //    user = new Core.Domain.Users.User()
            //    {
            //        OpenId = requestMessage.FromUserName,
            //        CreateTime = DateTime.Now,
            //        LastActivityTime = DateTime.Now,
            //        Subscribe = 1,
            //        Channel = "WX"
            //    };
            //    this._userService.InsertUser(user);
            //}
            //else
            //{
            //    user.Subscribe = 1;
            //    this._userService.UpdateUser(user);
            //}

            //try
            //{
            //    AccessTokenContainer.Register(WXinConfig.WeixinAppId, WXinConfig.WeixinAppSecret);
            //    var atoken = AccessTokenContainer.GetAccessTokenResult(WXinConfig.WeixinAppId);

            //    var wxuser = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(atoken.access_token, requestMessage.FromUserName, Senparc.Weixin.Language.zh_CN);
            //    user.NickName = wxuser.nickname;
            //    user.City = wxuser.city;
            //    user.Country = wxuser.country;
            //    user.HeadImgUrl = wxuser.headimgurl;
            //    user.Province = wxuser.province;
            //    user.Sex = wxuser.sex;
            //    user.UnionId = wxuser.unionid;
            //    this._userService.UpdateUser(user);
            //}
            //catch(Exception ex)
            //{
            //    this._logger.Error(ex.Message, ex, user);
            //}
            //var accesstokenresult = OAuthApi.GetAccessToken(WXinConfig.WeixinAppId, WXinConfig.WeixinAppSecret, code);
            //OAuthApi.GetUserInfo()
            //responseMessage.Content = GetWelcomeInfo();
            //responseMessage.Content = "redirect=UserOAuth/Authentication";
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