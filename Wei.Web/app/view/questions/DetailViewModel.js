Ext.define('Wei.view.questions.DetailViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.questions_detail',

    data: {
        questionBankModel: null
    },

    stores: {
        questionlist: {
            type: 'questions_question',
            autoLoad: false
        },
        questionitemlist: {
            type: 'questions_questionitem',
            autoLoad: false
        },
        questionanswerlist: {
            type: 'questions_questionanswer',
            autoLoad: false
        },


        // 答题方式
        answertypelist: {
            type: 'store',
            data: [
                { code: 'SingleCheck', name: '单选' },
                { code: 'MultiCheck', name: '多选' },
                { code: 'Text', name: '简答' },
                { code: 'End', name: '结束' }
            ]
        },
        // 出题类型
        questiontypelist: {
            type: 'store',
            data: [
                { code: 'Text', name: '文本' },
                { code: 'Image', name: '图片' }
            ]
        }
    }
});
