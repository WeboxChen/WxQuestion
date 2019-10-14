Ext.define('Wei.view.questions.DetailViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.questions_detail',

    data: {
        questionBankModel: null
    },

    stores: {
        questionlist: {
            type: 'questions_question',
            autoLoad: false,
            //_dataView: 'V_Question_Main'
        },
        //questionsublist: {
        //    type: 'questions_question',
        //    autoLoad: false
        //},
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
                { code: 'Tips', name: '提示' },
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
        },
        // 匹配类型
        matchingtypelist: {
            type: 'store',
            data: [
                { code: 'Single', name: '单结果匹配' },
                { code: 'Multiple', name: '多结果匹配' }
            ]
        }
    }
});
