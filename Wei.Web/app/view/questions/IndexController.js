Ext.define('Wei.view.questions.IndexController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.questions',

    // common
    setCurrentView: function (view, params) {
        var contentPanel = this.getView();

        //We skip rendering for the following scenarios:
        // * There is no contentPanel
        // * view xtype is not specified
        // * current view is the same
        if (!contentPanel || view === '' || (contentPanel.down() && contentPanel.down().xtype === view)) {
            return false;
        }

        if (params && params.openWindow) {
            var cfg = Ext.apply({
                xtype: 'common_window',
                items: [
                    Ext.apply({
                        xtype: view
                    }, params.targetCfg)
                ]
            }, params.windowCfg);
            Ext.create(cfg);
        } else {
            Ext.suspendLayouts();
            contentPanel.removeAll(true);
            contentPanel.add(
                Ext.apply({
                    xtype: view
                }, params)
            );
            Ext.resumeLayouts(true);
        }
    },
    init: function () {
        this.setCurrentView('questions_questionbanklist');
    },
    onQuestionShow: function () {
        this.setCurrentView('questions_questionbanklist');
    },

    // question bank
    onDataQuery: function (t) {
        var that = this,
            view = that.getView().down('questions_questionbanklist'),
            grid = view.down('gird'),
            filterForm = view.down('#questionbank_form_filter'),
            data = filterForm.getValues(),
            store = that.getStore('questionbanklist');
        store.setRemoteFilter(true);
        that.sendFilter(store, data);
        //store.load();
    },
    onQuestionBankAdd: function (t) {
        var that = this;
        var store = that.getStore('questionbanklist');
        this.alertFormObjWindow('questions_questionbankinfo', '添加题卷', null, {}, function (values) {
            var flag = false;
            store.add(values);
            store.ypuSimpleSync({
                success: function () {
                    flag = true;
                    store.reload();
                    that.alertSuccessMsg('保存成功！');
                },
                failure: function () {
                    flag = false;
                }
            });
            return flag;
        });
    },
    onQuestionBankEdit: function (t) {
        var that = this,
            store = that.getStore('questionbanklist'),
            grid = t.up('grid'),
            selected = grid.getSelection();
        if (selected && selected.length == 1) {
            var record = selected[0];
            this.alertFormObjWindow('questions_questionbankinfo', '修改题卷', record, { }, function (values) {
                store.ypuSimpleSync({
                    success: function () {
                        store.reload();
                    }
                });
                return true;
            });
        }
        
    },
    onQuestionBankView: function (t) {
        var that = this,
            grid = t.up('grid'),
            selection = grid.getSelection();
        if (selection.length == 0)
            return;
        var record = selection[0];
        that.setCurrentView('questions_detail', { _questionbank: record });
    },
    onQuestionBankDel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        that.confirmMsg('是否删除选中数据？', function () {
            store.remove(selection);
            store.ypuSimpleSync({
                success: function () {
                    store.reload();
                }
            });
        })
    },
    onRequestUserAnswer: function (t) {
        var that = this,
            grid = t.up('grid'),
            selection = grid.getSelection();
        if (selection.length == 0)
            return;

        var record = selection[0];
        if (!record.get('autoresponse')) {
            that.alertErrorMsg('请先设定题卷可以自动唤醒！');
            return;
        }

        if (record.get('status') == -1) {
            that.alertErrorMsg('题卷状态未启用！');
            return;
        }

        that.alertWindow('邀请答题（只能邀请24小时内有互动的用户）', {
            xtype: 'container',
            viewModel: 'account',

            listeners: {
                render: function (t) {
                    var userliststore = t.getViewModel().getStore('userlist');
                    userliststore.load();
                }
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'form',
                    cls: 'form_filter',
                    itemId: 'user_form_filter',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'textfield',
                        margin: '0 6 0 0',
                        height: 32
                    },
                    items: [
                        {
                            emptyText: '昵称',
                            name: 'nickname.like',
                            width: 120
                        },
                        {
                            xtype: 'combobox',
                            emptyText: '性别',
                            bind: {
                                store: '{sexstore}'
                            },
                            displayField: 'name',
                            valueField: 'value',
                            name: 'sex.=',
                            width: 120
                        },
                        {
                            emptyText: '城市',
                            name: 'city.=',
                            width: 120
                        },
                        {
                            xtype: 'combobox',
                            emptyText: '婚姻状况',
                            bind: {
                                store: '{marriedstore}'
                            },
                            displayField: 'name',
                            valueField: 'value',
                            name: 'married.=',
                            width: 120
                        },
                        {
                            xtype: 'datefield',
                            emptyText: '最后答题时间起',
                            name: 'lastanswertime.>='
                        },
                        {
                            xtype: 'datefield',
                            emptyText: '最后答题时间至',
                            name: 'lastanswertime.<='
                        },
                        {
                            xtype: 'button',
                            text: '查 询',
                            handler: function (t) {
                                var form = t.up('form'),
                                    view = form.up('container'),
                                    values = form.getValues(),
                                    viewmodel = view.getViewModel(),
                                    userliststore = viewmodel.getStore('userlist');
                                that.sendFilter(userliststore, values);
                            }
                        }
                    ]
                },
                {
                    xtype: 'account_usergrid',
                    selModel: {
                        selType: 'checkboxmodel',
                        mode: 'MULTI',
                        showHeaderCheckbox: true,
                    },
                    bind: {
                        store: '{userlist}'
                    },
                    flex: 1
                }
            ]
        }, function (b) {
            var win = b.up('window'),
                grid = win.down('account_usergrid'),
                selection = grid.getSelection();
            if (selection.length == 0) {
                that.alertErrorMsg('请选中要推送的用户！');
                return;
            }
            var userids = [];
            selection.forEach(function (u, i) {
                userids.push(u.getId());
            });

            that.postJson('api/questionbank/requestuseranswer', {
                    userids: userids, questionbankid: record.getId()
                }, function () {
                    that.alertSuccessMsg('推送成功！');
                })
        });
    },

    // questionBank grid
    onQuestionBankGridDblClick: function (t, r, ele, ri, e, eopts) {
        var that = this;
        that.setCurrentView('questions_detail', { _questionbank: r });
    }
    
});
