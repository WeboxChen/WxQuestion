using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.CommonAPIs;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenusManager
{
    class Program
    {
        internal static string appId = System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"];
        internal static string appSecret = System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"];

        static void Main(string[] args)
        {
            //设置全局 Debug 状态
            var isGLobalDebug = true;
            var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);

            //CO2NET 全局注册，必须！！
            IRegisterService register = RegisterService.Start(senparcSetting)
                                          .UseSenparcGlobal(false, null);

            ///* 微信配置开始
            // * 建议按照以下顺序进行注册
            // */

            ////设置微信 Debug 状态
            var isWeixinDebug = true;
            var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);

            //微信全局注册，必须！！
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);
            //// 注册微信AppId
            //AccessTokenContainer.Register(appId, appSecret);
            AccessTokenContainer.Register(appId, appSecret);

            Console.WriteLine("begin");
            string content = Console.ReadLine();

            if(string.Equals(content, "Y"))
            {
                Console.WriteLine("get access token");
                var tokenresult = AccessTokenContainer.GetAccessTokenResult(appId, true);
                if(tokenresult.errcode == ReturnCode.请求成功)
                {
                    Console.WriteLine("delete menu");
                    try
                    {
                        // 删除原来的菜单
                        CommonApi.DeleteMenu(tokenresult.access_token);

                        Console.WriteLine("build ButtonGroup begin");
                        // 构建新菜单
                        ButtonGroup bg = new ButtonGroup();

                        //单击
                        bg.button.Add(new SingleClickButton()
                        {
                            name = "注册",
                            key = "btnRegister"
                        });

                        //二级菜单
                        var subButton = new SubButton()
                        {
                            name = "二级菜单"
                        };
                        subButton.sub_button.Add(new SingleClickButton()
                        {
                            key = "SubClickRoot_Text",
                            name = "返回文本"
                        });
                        subButton.sub_button.Add(new SingleClickButton()
                        {
                            key = "SubClickRoot_News",
                            name = "返回图文"
                        });
                        subButton.sub_button.Add(new SingleClickButton()
                        {
                            key = "SubClickRoot_Music",
                            name = "返回音乐"
                        });
                        subButton.sub_button.Add(new SingleViewButton()
                        {
                            url = "http://weixin.senparc.com",
                            name = "Url跳转"
                        });
                        bg.button.Add(subButton);
                        Console.WriteLine("build ButtonGroup end");
                        var result = CommonApi.CreateMenu(appId, bg);
                        Console.WriteLine("CreateMenu");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    
                }
                else
                {
                    Console.WriteLine($"errcode: {tokenresult.errcode}; errmsg: {tokenresult.errmsg}");
                }
                //// 获取原来的菜单
                //var result = CommonApi.GetMenu(accessToken);

                //Console.WriteLine($"accessToken: {accessToken}");

                


                Console.ReadLine();
            }

        }
    }
}
