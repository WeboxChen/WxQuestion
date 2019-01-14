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
                    emptyText: '标题',
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