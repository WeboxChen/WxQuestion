Ext.define('Wei.view.questions.QuestionBankInfo', {
    extend: 'Ext.form.Panel',
    alias: 'widget.questions_questionbankinfo',

    layout: 'column',
    defaults: {
        columnWidth: '0.3',
        padding: '10 10 5 10',
        labelAlign: 'right'
    },
    listeners: {

        afterRender: function () {
            var me = this;
            if (me._record) {
                me.loadRecord(me._record);
            }
        },
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
            xtype: 'checkboxfield',
            fieldLabel: '自动唤醒',
            name: 'autoresponse'
        },

        {
            xtype: 'textfield',
            fieldLabel: '响应语句',
            name: 'responsekeywords'
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