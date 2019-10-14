Ext.define('Wei.view.setting.PropertySetting', {
    extend: 'Ext.container.Container',
    alias: 'widget.setting_basic',

    controller: 'setting_property',
    viewModel: 'setting_property',

    listeners: {
        render: 'onAccountUserListRender'
    },

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    flex: 1,

    items: [
        {
            xtype: 'form',
            cls: 'form_filter',
            itemId: 'user_form_filter',
            hidden: true,
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
                    xtype: 'button',
                    text: '查 询',
                    handler: 'onDataQuery'
                }
            ]
        },
        {
            xtype: 'container',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            flex: 1,
            items: [
                {
                    xtype: 'setting_propertygrid',
                    title: '基础属性',
                    tbar: [
                        {
                            text: '修改',
                            handler: 'onEdit'
                        }
                    ],
                    bind: {
                        store: '{propertystore}'
                    },
                    flex: 1,
                    split: true,
                    listeners: {
                        select: 'onPropertySelect'
                    }
                }, {
                    xtype: 'setting_propertyvaluegrid',
                    title: '基础属性值',
                    tbar: [
                        {
                            text: '添加',
                            handler: 'onPropertyValueAdd'
                        }, {
                            xtype: 'splitbutton',
                            text: '修改',
                            handler: 'onEdit',
                            menu: {
                                items: [
                                    {
                                        text: '单项修改',
                                        handler: 'onEdit'
                                    },
                                    {
                                        text: '批量修改',
                                        handler: 'onMultiEdit'
                                    }
                                ]
                            }
                        }, {
                            text: '删除',
                            handler: 'onDel'
                        }, {
                            text: '取消',
                            handler: 'onCancel'
                        }, {
                            text: '保存',
                            handler: 'onSave'
                        }
                    ],
                    bind: {
                        store: '{propertyvaluestore}'
                    },
                    flex: 1,
                    split: true
                }
            ]
        }
    ]
});