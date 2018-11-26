Ext.define('Wei.view.questions.QuestionBankInfo', {
    extend: 'Ext.form.Panel',
    alias: 'widget.questions_questionbankinfo',

    layout: 'column',
    defaults: {
        columnWidth: '0.3',
        padding: '10 10 5 10',
        labelAlign: 'right'
    },

    items: [
        {
            xtype: 'combobox',
            fieldLabel: '类型',
            name: 'type',
            bind: {
                store: '{moduletypelist}'
            },
            editable: false,
            queryMode: 'local',
            displayField: 'name',
            valueField: 'id'
        },
        {
            xtype: 'textfield',
            fieldLabel: '标题',
            name: 'title'
        },
        {
            xtype: 'textfield',
            fieldLabel: '描述',
            name: 'description'
        },
        {
            xtype: 'textfield',
            fieldLabel: '备注',
            name: 'remark'
        },
        {
            xtype: 'combobox',
            fieldLabel: '状态',
            name: 'status',
            bind: {
                store: '{statestore}'
            },
            editable: false,
            queryMode: 'local',
            displayField: 'name',
            valueField: 'id',
            value: 0
        }
    ]
});