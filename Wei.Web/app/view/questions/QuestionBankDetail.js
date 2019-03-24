Ext.define('Wei.view.questions.QuestionBankDetail', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.questions_detail',

    controller: 'questions_detail',
    viewModel: 'questions_detail',

    tabBarHeaderPosition: 0,
    tools: [
        {
            type: 'close',
            callback: 'onBackBtnClick'
        }
    ],

    layout: 'fit',
    items: [
        {
            title: '基础信息',
            xtype: 'questions_questionbankinfo',
            listeners: {
                render: 'onQuestionBankInfoRender'
            },
            bbar: [
                '->',
                {
                    xtype: 'button',
                    text: '保存',
                    handler: 'onQuestionBankInfoSave'
                }
            ]
        },
        {
            title: '问卷题目',
            xtype: 'questions_questionlist'
        }
    ],

    listeners: {
        beforerender: 'onDetailBeforeRender'
    }
    //initComponent: function () {
    //    this.on({
    //        //afterrender: 'displayBtnFunc',
    //        beforerender: 'displayChildrenModuleByTab',
    //    });
    //    // 延迟0.3秒， 数据是否可以编辑
    //    this.on({
    //        afterrender: {
    //            fn: 'onOrderDetailAfterRenderByStatus',
    //            delay: 300
    //        }
    //    });
    //    this.callParent();
    //}
});
