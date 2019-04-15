using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Wei.Web
{
    public static class WXinConfig
    {
        private static string _defaultCacheNamespace;
        /// <summary>
        /// 
        /// </summary>
        public static string DefaultCacheNamespace
        {
            get { return _defaultCacheNamespace ?? (_defaultCacheNamespace = ConfigurationManager.AppSettings["DefaultCacheNamespace"]); }
        }


        private static string _weixinToken;
        /// <summary>
        /// 微信公众号对接Token
        /// </summary>
        public static string WeixinToken
        {
            get { return _weixinToken ?? (_weixinToken = ConfigurationManager.AppSettings["WeixinToken"]); }
        }

        private static string _weixinEncodingAESKey;
        /// <summary>
        /// 消息解密秘钥
        /// </summary>
        public static string WeixinEncodingAESKey
        {
            get { return _weixinEncodingAESKey ?? (_weixinEncodingAESKey = ConfigurationManager.AppSettings["WeixinEncodingAESKey"]); }
        }

        private static string _weixinAppId;
        /// <summary>
        /// 开发者AppId
        /// </summary>
        public static string WeixinAppId
        {
            get { return _weixinAppId ?? (_weixinAppId = ConfigurationManager.AppSettings["WeixinAppId"]); }
        }

        private static string _weixinAppSecret;
        /// <summary>
        /// 开发者密码
        /// </summary>
        public static string WeixinAppSecret
        {
            get { return _weixinAppSecret ?? (_weixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"]); }
        }

        private static string _mediaDir;
        public static string MediaDir
        {
            get { return _mediaDir ?? (_mediaDir = ConfigurationManager.AppSettings["MediaDir"]); }
        }

        private static string _invitationText;
        public static string InvitationText
        {
            get { return _invitationText ?? (_invitationText = ConfigurationManager.AppSettings["InvitationText"]); }
        }
    }
}