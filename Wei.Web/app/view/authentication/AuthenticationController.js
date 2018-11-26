Ext.define('Wei.view.authentication.AuthenticationController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.authentication',

    //TODO: implement central Facebook OATH handling here

    // onFaceBookLogin : function() {
    //     this.redirectTo('dashboard', true);
    // },

    onLoginButton: function (t) {
        // ? 可以尝试用 AuthenticationModel 获取值
        var loginForm = t.findParentByType('form');
        var values = loginForm.getValues();
        var that = this;

        /*
                var users = Ext.create('Wei.data.base.User');
                var islogin = false;
                var user;
                Ext.each(users.data, function (u) {
                    if (u.LoginName == values['userid'] && u.Password == values['password']) {
                        user = u;
                        islogin = true;
                    }
                });
                if (!islogin) {
                    that.alertErrorMsg('账号或密码错误哦！');
                    return;
                }
                var store = new Ext.util.LocalStorage({ id: 'Wei' });
                store.setItem('Permission', Ext.util.Base64.encode(JSON.stringify(user)));
                that.loadBaseData();
                that.redirectTo('setting_moduletypegrid');
        */

        var logindata = { LoginName: values['userid'], Password: md5(values['password']) };
        that.postJson('Authentication/Login', logindata,
            function (obj, response, opts) {
                // 保存数据
                var store = new Ext.util.LocalStorage({
                    id: 'Wei'
                });

                store.setItem("Permission", Ext.util.Base64.encode(JSON.stringify(obj)));
                that.loadBaseData();
                that.redirectTo('questions');

                // 加载基础数据
                //        that.loadBaseData();
                //        //that.mainModelLoad();
                //        that.loadModules();
                //        //that.redirectTo(that.loadModules());
                //        //var target = 'order';
                //        //if (that.IsClientUser())
                //        //    target = 'client';
                //        //that.redirectTo(target);
            },
            function () {

            }
        );
    },

    onLoginViewRender: function (t, eOpts) {
        var authentication = Ext.util.Cookies.get("authentication");
        var data;
        if (authentication)
            data = JSON.parse(authentication);
        if (data) {
            var comp = t.child('form');
            if (comp) {
                comp.getViewModel().setData(data);
            }
        }
    },
    onLockscreenViewRender: function (t, eOpts) {
        var authentication = Ext.util.Cookies.get("authentication");
        var data;
        if (authentication)
            data = JSON.parse(authentication);
        if (data) {
            data.password = '';
            Ext.util.Cookies.set("authentication", JSON.stringify(data));
        }
    }
});