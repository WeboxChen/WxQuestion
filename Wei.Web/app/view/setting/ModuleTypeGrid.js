Ext.define('Wei.view.setting.ModuleTypeGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.setting_moduletypegrid',

    controller: 'setting_moduletype',

    bind: {
        store: '{moduletypelist}',
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
        { xtype: 'gridcolumn', text: '题库类型', dataIndex: 'name', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '备注', dataIndex: 'remark', hidden: false, editor: { xtype: 'textfield' } }
    ]
});