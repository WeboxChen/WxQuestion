Ext.define('Wei.view.account.UserAttributeGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.account_userattributegrid',

    controller: 'account_userattribute',
    //viewModel: 'account_userattribute',
    bind: {
        store: '{userattributelist}',
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
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true },
        { xtype: 'gridcolumn', text: '代码', dataIndex: 'name', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '显示名称', dataIndex: 'displayname', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '标签', dataIndex: 'tags', hidden: false, editor: { xtype: 'textfield' } },


        { xtype: 'numbercolumn', text: '序号', dataIndex: 'displayorder', format: '0', hidden: false, editor: { xtype: 'numberfield' } },

    ]
});