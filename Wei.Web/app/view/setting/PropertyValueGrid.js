Ext.define('Wei.view.setting.PropertyValueGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.setting_propertyvaluegrid',
    

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true },
        { xtype: 'numbercolumn', text: '属性', dataIndex: 'propertyid', format: '0', hidden: true },
        {
            xtype: 'gridcolumn', text: '值', dataIndex: 'text', format: '0', hidden: false, width: 320,
            editor: {
                xtype: 'textfield'
            }
        },
        {
            xtype: 'numbercolumn', text: '默认', dataIndex: 'isdef', hidden: false, width: 80,
            editor: {
                field: {
                    xtype: 'checkbox',
                    inputValue: '1'
                },
                listeners: {
                    complete: function (t, v, sv, eopts) {
                        if (v)
                            t.context.record.set('isdef', 1);
                        else
                            t.context.record.set('isdef', 0);
                    }
                }
            },
            renderer: function (v, m, record) {
                if (v == 1)
                    return '<span class="fa fa-check"></span>';
                return '<span class="fa fa-remove"></span>';
            }
        },
        {
            xtype: 'numbercolumn', text: '序号', dataIndex: 'sort', format: '0', hidden: false, width: 80,
            editor: {
                xtype: 'numberfield'
            }
        }

    ]
});