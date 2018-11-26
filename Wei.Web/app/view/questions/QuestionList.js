Ext.define('Wei.view.questions.QuestionList', {
    extend: 'Ext.container.Container',
    alias: 'widget.questions_questionlist',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'questions_questiongrid',

            bind: {
                store: '{questionlist}',
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
            listeners: {
                render: 'onQuestionListRender'
            }
        }
    ]
});