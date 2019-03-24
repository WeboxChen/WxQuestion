Ext.define('Wei.view.questions.QuestionBankGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.questions_questionbankgrid',

    //controller: 'questions_questionbank',
    //viewModel: 'questions_basemodel',
    //bind: {
    //    store: '{questionbanklist}',
    //},

    //tbar: [
    //    {
    //        text: '添加',
    //        handler: 'onAdd'
    //    }, {
    //        xtype: 'splitbutton',
    //        text: '修改',
    //        handler: 'onEdit',
    //        menu: {
    //            items: [
    //                {
    //                    text: '单项修改',
    //                    handler: 'onEdit'
    //                },
    //                {
    //                    text: '批量修改',
    //                    handler: 'onMultiEdit'
    //                }
    //            ]
    //        }
    //    }, {
    //        text: '删除',
    //        handler: 'onDel'
    //    }, {
    //        text: '取消',
    //        handler: 'onCancel'
    //    }, {
    //        text: '保存',
    //        handler: 'onSave'
    //    }
    //],

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: false },
        {
            xtype: 'numbercolumn', text: '类型', dataIndex: 'type', format: '0', hidden: false,
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
        { xtype: 'gridcolumn', text: '标题', dataIndex: 'title', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '描述', dataIndex: 'description', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '备注', dataIndex: 'remark', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'datecolumn', text: '有效期起始', dataIndex: 'expiredatebegin', format: 'Y-m-d', hidden: false },
        { xtype: 'datecolumn', text: '有效期结束', dataIndex: 'expiredateend', format: 'Y-m-d', hidden: false },
        { xtype: 'booleancolumn', text: '自动唤醒', dataIndex: 'autoresponse', trueText: '√', falseText: '', width: 100, hidden: false, editor: { xtype: 'checkboxfield' } },
        { xtype: 'gridcolumn', text: '唤醒语句', dataIndex: 'responsekeywords', width: 160, hidden: false, editor: { xtype: 'textfield' } },
        {
            xtype: 'numbercolumn', text: '状态', dataIndex: 'status', format: '0', hidden: false,
            renderer: function (d) {
                if (d === 0)
                    return '启用'
                else if (d === -1)
                    return '禁用';
                return '';
            }
        },
        { xtype: 'datecolumn', text: '创建时间', dataIndex: 'createtime', format: 'Y-m-d H:i', hidden: false },
        { xtype: 'numbercolumn', text: '创建人', dataIndex: 'creatorid', format: '0', hidden: false }
    ]
});