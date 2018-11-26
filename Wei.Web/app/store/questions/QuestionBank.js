Ext.define('Wei.store.questions.QuestionBank', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.questions_questionbank',
    model: 'Wei.model.questions.QuestionBank',

    _dataView: 'Q_QuestionBank',
    _operateTblName: 'Q_QuestionBank'

});
