Ext.define('Wei.view.authentication.LockScreen', {
    extend: 'Wei.view.authentication.LockingWindow',
    xtype: 'lockscreen',
    alias: 'widget.lockscreen',
    
    _c_description: 'It\'s been a while. please enter your password to resume',
    _c_btnLogin: 'Login',
    _c_title: 'Session Expired',

    title: 'Session Expired',

    defaultFocus : 'authdialog',  // Focus the Auth Form to force field focus as well

    listeners: {
        beforerender: 'onLockscreenViewRender'
    },

    initComponent: function(){
        Ext.apply(this, {
            title: this._c_title,
            items: [
                {
                    xtype: 'authdialog',
                    reference: 'authDialog',
                    defaultButton : 'loginButton',
                    autoComplete: false,
                    width: 455,
                    cls: 'auth-dialog-login',
                    defaultFocus : 'textfield[inputType=password]',
                    layout: {
                        type  : 'vbox',
                        align : 'stretch'
                    },

                    items: [
                        {
                            xtype: 'container',
                            cls: 'auth-profile-wrap',
                            height : 120,
                            layout: {
                                type: 'hbox',
                                align: 'center'
                            },
                            items: [
                                {
                                    xtype: 'image',
                                    height: 80,
                                    margin: 20,
                                    width: 80,
                                    alt: 'lockscreen-image',
                                    cls: 'lockscreen-profile-img auth-profile-img',
                                    src: 'resources/images/user-profile/niming.jpg'
                                },
                                {
                                    xtype: 'box',
                                    html: '<div class=\'user-name-text\'> Wei </div><div class=\'user-post-text\'> 型录工程师 </div>'
                                }
                            ]
                        },

                        {
                            xtype: 'container',
                            padding: '0 20',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },

                            defaults: {
                                margin: '10 0'
                            },

                            items: [
                                {
                                    xtype: 'textfield',
                                    labelAlign: 'top',
                                    cls: 'lock-screen-password-textbox',
                                    labelSeparator: '',
                                    fieldLabel: this._c_description,
                                    name: 'password',
                                    emptyText: 'Password',
                                    inputType: 'password',
                                    allowBlank: false,
                                    triggers: {
                                        glyphed: {
                                            cls: 'trigger-glyph-noop password-trigger'
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    reference: 'loginButton',
                                    scale: 'large',
                                    ui: 'soft-blue',
                                    iconAlign: 'right',
                                    iconCls: 'x-fa fa-angle-right',
                                    text: this._c_btnLogin,
                                    formBind: true,
                                    listeners: {
                                        click: 'onLoginButton'
                                    }
                                // },
                                // {
                                //     xtype: 'component',
                                //     html: '<div style="text-align:right">' +
                                //         '<a href="#login" class="link-forgot-password">'+
                                //             'or, sign in using other credentials</a>' +
                                //         '</div>'
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        this.callParent(arguments);
    }

    
});
