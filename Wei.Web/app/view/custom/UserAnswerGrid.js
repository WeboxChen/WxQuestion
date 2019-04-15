Ext.define('Wei.view.custom.UserAnswerGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.custom_useranswergrid',

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: false },
        { xtype: 'gridcolumn', text: '用户', dataIndex: 'nickname', hidden: false },
        { xtype: 'gridcolumn', text: '题卷类型', dataIndex: 'questionbanktypename', hidden: false },
        { xtype: 'gridcolumn', text: '题卷', dataIndex: 'questionbank', hidden: false },
        {
            xtype: 'datecolumn',
            text: '开始时间',
            dataIndex: 'begintime',
            format: 'Y-m-d H:i',
            hidden: false,
            width: 140
        },
        {
            xtype: 'datecolumn',
            text: '完成时间',
            dataIndex: 'completedtime',
            format: 'Y-m-d H:i',
            hidden: false,
            width: 140
        },
        {
            xtype: 'gridcolumn',
            text: '状态',
            dataIndex: 'status'
        }
    ]
});