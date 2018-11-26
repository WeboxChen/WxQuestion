Ext.define('Wei.view.answers.UserAnswerGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.answers_useranswergrid',

    controller: 'answers_useranswer',
    viewModel: 'answers_basemodel',
    bind: {
        store: '{useranswerlist}',
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





    ]
});