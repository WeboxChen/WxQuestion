Ext.define('Wei.view.setting.QuestionBankTagGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.setting_questionbanktaggrid',
    
    controller: 'setting_questionbanktag',

    bind: {
        store: '{questionbanktaglist}',
    },

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

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: false },
        { xtype: 'gridcolumn', text: '标签', dataIndex: 'name', hidden: false, editor: { xtype: 'textfield' } },
        {
            xtype: 'numbercolumn', text: '标签类型', dataIndex: 'type', format: '0', hidden: false,
            editor: {
                xtype: 'combobox',
                bind: {
                    store: '{moduletypelist}'
                },
                editable: false,
                queryMode: 'local',
                displayField: 'name',
                valueField: 'id'
            },
            renderer: function (d, m, r) {
                var store = Ext.getCmp('mainView').getViewModel().getStore('moduletypelist'); // this.up('main') .getStore('moduletypelist');
                var moduletype = store.getById(d);
                if (moduletype)
                    return moduletype.get('name');
                return '';
            }
        },
        { xtype: 'gridcolumn', text: '描述', dataIndex: 'description', hidden: false, editor: { xtype: 'textfield' } }
    ]
});