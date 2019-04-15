Ext.define('Wei.view.questions.QuestionAnswerGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.questions_questionanswergrid',
    
    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true, width: 80 },
        { xtype: 'numbercolumn', text: '序号', dataIndex: 'sort', hidden: false, editor: { xtype: 'numberfield' }, width: 80 },
        { xtype: 'gridcolumn', text: '答案描述', dataIndex: 'answerdesc', hidden: true, editor: { xtype: 'textfield' }, width: 220 },
        { xtype: 'gridcolumn', text: '答案关键字', dataIndex: 'answerkeys', hidden: false, editor: { xtype: 'textfield' }, width: 220 },
        { xtype: 'numbercolumn', text: '下个问题', dataIndex: 'next', hidden: false, editor: { xtype: 'numberfield' }, width: 80 }
    ]
});