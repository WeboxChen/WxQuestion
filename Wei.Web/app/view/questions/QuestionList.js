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
            //maxWidth: 420,
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
            xtype: 'container',
            flex: 1,
            split: true,

            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'questions_questionitemgrid',
                    title: '选择题答案',
                    bind: {
                        store: '{questionitemlist}',
                    },
                    flex: 1,
                    split: true,

                    tbar: [
                        {
                            text: '添加',
                            handler: 'onQuestionItemAdd'
                        }, {
                            text: '修改',
                            handler: 'onQuestionItemEdit'
                        }, {
                            text: '删除',
                            handler: 'onQuestionItemDel'
                        }, {
                            text: '取消',
                            handler: 'onQuestionItemCancel'
                        }, {
                            text: '保存',
                            handler: 'onQuestionItemSave'
                        }
                    ],
                },
                {
                    xtype: 'questions_questionanswergrid',
                    title: '题目答案',
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
                }

            ]
        }
        
    ]
});