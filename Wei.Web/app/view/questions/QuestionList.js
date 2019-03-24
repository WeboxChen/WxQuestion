Ext.define('Wei.view.questions.QuestionList', {
    extend: 'Ext.container.Container',
    alias: 'widget.questions_questionlist',

    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    listeners: {
        render: 'onQuestionListRender',
    },

    items: [
        {
            xtype: 'questions_questiongrid',
            selModel: {
                selType: 'checkboxmodel',
                mode: 'SINGLE',
                showHeaderCheckbox: true
            },
            split: true,
            maxWidth: 420,
            minWidth: 220,
            bind: {
                store: '{questionlist}',
            },
            flex: 1,

            tbar: [
                {
                    text: '添加',
                    handler: 'onQuestionAdd'
                }, {
                    text: '修改',
                    handler: 'onQuestionEdit'
                }, {
                    text: '删除',
                    handler: 'onQuestionDel'
                }, {
                    text: '取消',
                    handler: 'onQuestionCancel'
                }, {
                    text: '保存',
                    handler: 'onQuestionSave'
                }, {
                    xtype: 'form',
                    listeners: {
                        afterrender: 'onFormAfterrender'
                    },
                    bodyPadding: '12 0 0',
                    name: 'questionlist_frm_upload',
                    items: [
                        {
                            xtype: 'hiddenfield',
                            name: 'QuestionBank_Id',
                            bind: {
                                value: '{QuestionBank_Id}'
                            }
                        },
                        {
                            xtype: 'fileuploadfield',
                            width: 50,
                            buttonOnly: true,
                            hideLabel: true,
                            //disabled: true,
                            buttonText: '导入',
                            name: 'frm_btn_import',
                            listeners: {
                                change: "onQuestionListImport"
                            }
                        }
                    ],
                }
            ],
            listeners: {
                select: 'onQuestionGridSelect',
                deselect: 'onQuestionGridDeselect'
            }
        },
        {
            xtype: 'questions_questionanswergrid',
            bind: {
                store: '{questionanswerlist}'
            },
            flex: 1,
            split: true,
            tbar: [
                {
                    text: '添加',
                    handler: 'onQuestionAnswerAdd'
                }, {
                    text: '修改',
                    handler: 'onQuestionAnswerEdit'
                }, {
                    text: '删除',
                    handler: 'onQuestionAnswerDel'
                }, {
                    text: '取消',
                    handler: 'onQuestionAnswerCancel'
                }, {
                    text: '保存',
                    handler: 'onQuestionAnswerSave'
                }
            ]
        },
    ]
});