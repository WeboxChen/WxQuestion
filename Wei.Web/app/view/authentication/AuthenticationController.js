Ext.define('Wei.view.authentication.AuthenticationController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.authentication',

    //TODO: implement central Facebook OATH handling here

    // onFaceBookLogin : function() {
    //     this.redirectTo('dashboard', true);
    // },

    onLoginButton: function (t) {
        // ? 可以尝试用 AuthenticationModel 获取值
        var that = this,
            loginForm = t.findParentByType('form'),
            values = loginForm.getValues();

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