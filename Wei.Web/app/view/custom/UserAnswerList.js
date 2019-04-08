Ext.define('Wei.view.custom.UserAnswerList', {
    extend: 'Ext.container.Container',
    alias: 'widget.custom_useranswerlist',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'form',
            cls: 'form_filter',
            itemId: 'useranswer_form_filter',
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
                    //fieldLabel: '用户',
                    //labelWidth: 60,
                    emptyText: '用户',
                    name: 'nickname.like',
                    width: 120
                },
                {
                    //fieldLabel: '题库标题',
                    //labelWidth: 60,
                    emptyText: '题库标题',
                    name: 'title.like',
                    width: 120
                },
                {
                    xtype: 'combobox',
                    emptyText: '类别',
                    bind: {
                        store: '{moduletypelist}'
                    },
                    name: 'type.=',
                    editable: false,
                    queryMode: 'local',
                    displayField: 'name',
                    valueField: 'id'
                },
                {
                    xtype: 'button',
                    text: '查 询',
                    handler: 'onDataQuery'
                }
            ]
        },
        {
            xtype: 'custom_useranswergrid',
            tbar: [
                {
                    text: '查看',
                    handler: 'onUserAnswerView'
                },
                {
                    text: '作废',
                    handler: 'onUserAnswerDiscard'
                }
            ],
            bind: {
                store: '{useranswerlist}'
            },
            flex: 1,
            listeners: {
                rowdblclick: 'onUserAnswerDblClick'
            }
        }
    ]
});