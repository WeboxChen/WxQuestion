﻿Ext.define('Wei.view.questions.QuestionBankList', {
    extend: 'Ext.container.Container',
    alias: 'widget.questions_questionbanklist',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    items: [
        {
            xtype: 'form',
            cls: 'form_filter',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            defaults: {
                xtype: 'textfield',
                margin: '0 6 0 0',
                height: 32
            },
            items: [
                {
                    emptyText: '标题',
                    name: 'title.like',
                    width: 120
                },
                {
                    xtype: 'combobox',
                    emptyText: '类别',
                    bind: {
                        store: '{moduletypelist}'
                    },
                    name: 'type.=',
                    editable: false,
                    queryMode: 'local',
                    displayField: 'name',
                    valueField: 'id'
                },
                {
                    emptyText: '标签',
                    xtype: 'tagfield',
                    bind: {
                        store: '{questionbanktaglist}'
                    },
                    name: 'tag.in',
                    editable: false,
                    queryMode: 'local',
                    multiSelect: true,
                    displayField: 'name',
                    valueField: 'id',
                    filterPickList: true
                },
                {
                    xtype: 'button',
                    text: '查 询',
                    handler: 'onDataQuery'
                }
            ]
        },
        {
            xtype: 'questions_questionbankgrid',
            tbar: [
                {
                    text: '新增',
                    handler: 'onQuestionBankAdd'
                },
                {
                    text: '修改',
                    handler: 'onQuestionBankEdit'
                },
                {
                    text: '查看',
                    handler: 'onQuestionBankView'
                },
                {
                    text: '删除',
                    handler: 'onQuestionBankDel'
                }
            ],
            bind: {
                store: '{questionbanklist}'
            },
            flex: 1,
            listeners: {
                rowdblclick: 'onQuestionBankGridDblClick'
            }
        }
    ]
});