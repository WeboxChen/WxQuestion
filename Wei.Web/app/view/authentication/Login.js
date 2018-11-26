Ext.define('Wei.view.authentication.Login', {
    extend: 'Wei.view.authentication.LockingWindow',
    // xtype: 'login',
    alias: 'widget.login',

    _c_title: 'Let\'s Log In',
    _c_userid: 'User id',
    _c_password: 'Password',
    _c_ptext: 'Sign into your account',
    _c_remember: 'Remember me',
    _c_forgot: 'Forgot Password ?',
    _c_btnLogin: 'Login',

    defaultFocus: 'authdialog', // Focus the Auth Form to force field focus as well

    listeners: {
        beforerender: 'onLoginViewRender'
    },

    initComponent: function() {
        this.addCls('user-login-register-container');

        Ext.apply(this, {
            title: this._c_title,

            items: [
                {
                    xtype: 'authdialog',
                    defaultButton : 'loginButton',
                    autoComplete: true,
                    bodyPadding: '20 20',
                    cls: 'auth-dialog-login',
                    header: false,
                    width: 415,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },

                    defaults : {
                        margin : '5 0'
                    },

                    items: [
                        {
                            xtype: 'label',
                            text: this._c_ptext
                        },
                        {
                            xtype: 'textfield',
                            cls: 'auth-textbox',
                            name: 'userid',
                            bind: '{userid}',
                            height: 55,
                            hideLabel: true,
                            allowBlank : false,
                            emptyText: this._c_userid,
                            triggers: {
                                glyphed: {
                                    cls: 'trigger-glyph-noop auth-email-trigger'
                                }
                            }
                        },
                        {
                            xtype: 'textfield',
                            cls: 'auth-textbox',
                            height: 55,
                            hideLabel: true,
                            emptyText: this._c_password,
                            inputType: 'password',
                            name: 'password',
                            bind: '{password}',
                            allowBlank : false,
                            triggers: {
                                glyphed: {
                                    cls: 'trigger-glyph-noop auth-password-trigger'
                                }
                            }
                        },
                        //{
                        //    xtype: 'container',
                        //    layout: 'hbox',
                        //    items: [
                        //        {
                        //            xtype: 'checkboxfield',
                        //            flex : 1,
                        //            cls: 'form-panel-font-color rememberMeCheckbox',
                        //            height: 30,
                        //            bind: '{persist}',
                        //            boxLabel: this._c_remember
                        //        },
                        //        //{
                        //        //    xtype: 'box',
                        //        //    html: '<a href="#passwordreset" class="link-forgot-password"> '+ this._c_forgot + '</a>'
                        //        //}
                        //    ]
                        //},
                        {
                            xtype: 'button',
                            reference: 'loginButton',
                            scale: 'large',
                            ui: 'soft-green',
                            iconAlign: 'right',
                            iconCls: 'x-fa fa-angle-right',
                            text: this._c_btnLogin,
                            formBind: true,
                            listeners: {
                                click: 'onLoginButton'
                            }
                        }
                    ]
                }
            ]
        })
        this.callParent(arguments);
    }
});