Ext.define('Wei.view.setting.WordsSubstitutionGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.setting_wordssubstitutiongrid',

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', width: 60, hidden: false },
        {
            xtype: 'numbercolumn', text: '所属题卷', dataIndex: 'questionbankid', format: '0', width: 160, hidden: false,
            renderer: function (d, m, r) {
                return r.get('questionbankname');
            }
        },
        {
            xtype: 'gridcolumn', text: '匹配词语', dataIndex: 'words1', hidden: false, width: 140, editor: { xtype: 'textfield' }
        },
        { xtype: 'gridcolumn', text: '替换词语', dataIndex: 'words2', hidden: false, width: 140, editor: { xtype: 'textfield' } }

    ]
});