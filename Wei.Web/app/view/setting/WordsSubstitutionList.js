Ext.define('Wei.view.setting.WordsSubstitutionList', {
    extend: 'Ext.container.Container',
    alias: 'widget.setting_wordssubtitutionlist',

    controller: 'setting_wordssubstitution',
    viewModel: 'setting_wordssubstitution',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    listeners: {
        render: 'onWordsSubstitutionListRender',
    },
    items: [
        {
            xtype: 'setting_wordssubstitutiongrid',
            
            split: true,
            bind: {
                store: '{wordssubstitutionlist}',
            },
            flex: 1,
            tbar: [
                {
                    text: '添加',
                    handler: 'onAdd'
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
            listeners: {
                render: 'onWordsSubstitutionGridRender',
            }
        }
    ]
});