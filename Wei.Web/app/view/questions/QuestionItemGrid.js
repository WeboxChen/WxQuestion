Ext.define('Wei.view.questions.QuestionItemGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.questions_questionitemgrid',


    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true },

        { xtype: 'gridcolumn', text: '选项', dataIndex: 'code', width: 80, hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '选项文本', dataIndex: 'text', width: 260, hidden: false, editor: { xtype: 'textfield' } }
    ]
});