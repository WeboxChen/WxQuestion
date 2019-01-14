using System;
using System.Collections.Generic;
using System.Linq;
using Wei.Core.Infrastructure;
using Wei.Services.Logging;
using Wei.Services.Users;

namespace Wei.Web.ViewServices.Users
{
    public class UserViewService
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public UserViewService()
        {
            this._userService = EngineContext.Current.Resolve<IUserService>();
            this._logger = EngineContext.Current.Resolve<ILogger>();
        }

        public Wei.Core.Domain.Users.User SaveUser(string accesstokenOrAppid, string openid)
        {
            var user = _userService.GetUserById(openid);
            if(user == null)
            {
                user = new Core.Domain.Users.User()
                {
                    OpenId = openid,
                    CreateTime = DateTime.Now,
                    LastActivityTime = DateTime.Now,
                    Subscribe = 1,
                    Channel = "WX"
                };
                this._userService.InsertUser(user);
            }
            else
            {
                user.Subscribe = 1;
            }
            var wxuser = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Info(WXinConfig.WeixinAppId, openid, Senparc.Weixin.Language.zh_CN);
            user.NickName = wxuser.nickname;
            user.City = wxuser.city;
            user.Country = wxuser.country;
            user.HeadImgUrl = wxuser.headimgurl;
            user.Province = wxuser.province;
            user.Sex = wxuser.sex;
            user.UnionId = wxuser.unionid;
            user.Subscribe_Time = wxuser.subscribe_time;
            this._userService.UpdateUser(user);

            return user;
        }

        /// <summary>
        /// 获取最新的关注用户
        /// </summary>
        /// <param name="accesstokenOrAppid"></param>
        public void SaveUserList(string accesstokenOrAppid)
        {
            _logger.Information("SaveUserList Begin");
            List<Senparc.Weixin.MP.AdvancedAPIs.User.BatchGetUserInfoData> openuserlist = null;
            var lastuser = this._userService.GetLastWXinUser();
            _logger.Information(lastuser.OpenId);
            var openids = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Get(WXinConfig.WeixinAppId, lastuser == null? null: lastuser.OpenId);
            while(openids != null && openids.count > 0)
            {
                openuserlist = openids.data.openid.Select(x =>
                {
                    return new Senparc.Weixin.MP.AdvancedAPIs.User.BatchGetUserInfoData()
                    {
                        openid = x,
                        LangEnum = Senparc.Weixin.Language.zh_CN
                    };
                }).ToList();
                var userinfoResult = Senparc.Weixin.MP.AdvancedAPIs.UserApi.BatchGetUserInfo(WXinConfig.WeixinAppId, openuserlist, 30000);
                foreach(var userinfo in userinfoResult.user_info_list)
                {
                    var user = _userService.GetUserById(userinfo.openid);
                    if(user == null)
                    {
                        user = new Core.Domain.Users.User()
                        {
                            OpenId = userinfo.openid,
                            CreateTime = DateTime.Now,
                            LastActivityTime = DateTime.Now,
                            Subscribe = 1,
                            Channel = "WX",
                            NickName = userinfo.nickname,
                            City = userinfo.city,
                            Country = userinfo.country,
                            HeadImgUrl = userinfo.headimgurl,
                            Province = userinfo.province,
                            Sex = userinfo.sex,
                            UnionId = userinfo.unionid,
                            Subscribe_Time = userinfo.subscribe_time
                        };
                        this._userService.InsertUser(user);
                    }
                    else
                    {
                        user.NickName = userinfo.nickname;
                        user.City = userinfo.city;
                        user.Country = userinfo.country;
                        user.HeadImgUrl = userinfo.headimgurl;
                        user.Province = userinfo.province;
                        user.Sex = userinfo.sex;
                        user.UnionId = userinfo.unionid;
                        user.Subscribe_Time = userinfo.subscribe_time;
                        this._userService.UpdateUser(user);
                    }
                }

                openids = Senparc.Weixin.MP.AdvancedAPIs.UserApi.Get(WXinConfig.WeixinAppId, openids.next_openid);
            }

            _logger.Information("SaveUserList End");
        }
    }
}