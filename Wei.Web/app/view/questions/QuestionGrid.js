Ext.define('Wei.view.questions.QuestionGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.questions_questiongrid',

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true },
        { xtype: 'numbercolumn', text: '题号', dataIndex: 'sort', format: '0', hidden: false, editor: { xtype: 'numberfield' }, width: 60 },
        {
            xtype: 'gridcolumn', text: '答题类型', dataIndex: 'atype', width: 80, hidden: false, editor: {
                xtype: 'combobox',
                bind: {
                    store: '{answertypelist}'
                },
                editable: false,
                queryMode: 'local',
                displayField: 'name',
                valueField: 'code'
            },
            renderer: 'onQuestionGridATypeRenderer'
        },
        {
            xtype: 'gridcolumn', text: '题目类型', dataIndex: 'qtype', width: 80, hidden: false, editor: {
                xtype: 'combobox',
                bind: {
                    store: '{questiontypelist}'
                },
                editable: false,
                queryMode: 'local',
                displayField: 'name',
                valueField: 'code'
            },
            renderer: 'onQuestionGridQTypeRenderer'
        },
        { xtype: 'gridcolumn', text: '问题文本', dataIndex: 'text', width: 260, hidden: false, editor: { xtype: 'textfield' } },
        {
            xtype: 'gridcolumn', text: '问题图片', dataIndex: 'imagecode', hidden: true, editor: {
                xtype: 'filefield'
            }
        },
        { xtype: 'gridcolumn', text: '答案', dataIndex: 'answer', width: 160, hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'numbercolumn', text: '下一题（正确）', dataIndex: 'next1', width: 120, format: '0', hidden: false, editor: { xtype: 'numberfield' } },
        { xtype: 'numbercolumn', text: '下一题（错误）', dataIndex: 'next2', width: 120, format: '0', hidden: false, editor: { xtype: 'numberfield' } }
    ]
});