Ext.define('Wei.view.setting.PropertyGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.setting_propertygrid',

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true, width: 60 },
        { xtype: 'gridcolumn', text: '系统代码', dataIndex: 'pcode', hidden: false, width: 200 },
        { xtype: 'gridcolumn', text: '代码功能描述', dataIndex: 'desc', hidden: false, width: 320 },
        { xtype: 'numbercolumn', text: '值选择类型', dataIndex: 'selectvaluetype', format: '0', hidden: true, editor: { xtype: 'numberfield' } }
    ]
});