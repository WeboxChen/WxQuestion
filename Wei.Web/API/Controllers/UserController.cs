using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wei.Services.Users;
using Wei.Web.Framework.ExtJs;

namespace Wei.Web.API.Controllers
{
    /// <summary>
    /// 用户处理控制器
    /// </summary>
    public class UserController : ApiController
    {
        // 用户处理
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        ///// <summary>
        ///// 用户关注，记录用户信息
        ///// </summary>
        ///// <param name="requestMessage"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public IResponseMessageBase Follow(RequestMessageEvent_Subscribe requestMessage)
        //{
        //    // 获取用户详细信息
        //    //CommonApi.GetUserInfo(requestMessage.FromUserName, requestMessage.FromUserName);
        //    //// 保存用户
        //    //var user = _userService.GetUserById(requestMessage.FromUserName);
        //    //if(user!= null)
        //    //{
        //    //    // 更新用户为已关注
        //    //    user.Subscribe = 1;
        //    //    _userService.UpdateUser(user);
        //    //}
        //    //else
        //    //{
        //    //    user = new Core.Domain.Users.User()
        //    //    {
        //    //        Subscribe = 1,

        //    //    }
        //    //}
        //    //// requestMessage.FromUserName

        //    //// 根据openid获取用户信息
        //    //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
        //    //responseMessage.Content = "欢迎您";
        //    //return responseMessage;
        //}
    }
}
