Ext.define('Wei.store.questions.QuestionBank', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.questions_questionbank',
    model: 'Wei.model.questions.QuestionBank',

    _dataView: 'Q_QuestionBank',
    _operateTblName: 'Q_QuestionBank',
    pageSize: 25,

    proxy: {
        api: {
            create: 'api/questionbank/CreateQuestionBank',
            read: 'api/questionbank/GetQuestionBankList',
            update: 'api/questionbank/UpdateQuestionBank',
            destroy: 'api/questionbank/DestroyQuestionBank'
        }
    }

});
