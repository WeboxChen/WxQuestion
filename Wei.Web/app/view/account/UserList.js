Ext.define('Wei.view.account.UserList', {
    extend: 'Ext.container.Container',
    alias: 'widget.account_userlist',

    controller: 'account',
    viewModel: 'account',

    listeners: {
        render: 'onAccountUserListRender'
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
                //{
                //    xtype: 'datefield',
                //    emptyText: '最后答题时间起',
                //    bind: {
                //        store: '{moduletypelist}'
                //    },
                //    name: 'type.=',
                //    editable: false,
                //    queryMode: 'local',
                //    displayField: 'name',
                //    valueField: 'id'
                //},
                {
                    xtype: 'button',
                    text: '查 询',
                    handler: 'onDataQuery'
                }
            ]
        },
        {
            xtype: 'account_usergrid',
            tbar: [
                {
                    text: '修改',
                    handler: 'onEdit'
                //}, {
                //    text: '删除',
                //    handler: 'onDel'
                }, {
                    text: '取消',
                    handler: 'onCancel'
                }, {
                    text: '保存',
                    handler: 'onSave'
                }
                //, '-',
                //{
                //    text: '邀请答题',
                //    handler: 'onAnswerByRequest'
                //}
            ],
            bind: {
                store: '{userlist}'
            },
            flex: 1
        }
    ]
});