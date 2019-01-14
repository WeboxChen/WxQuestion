Ext.define('Wei.store.questions.QuestionAnswer', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.questions_questionanswer',
    model: 'Wei.model.questions.QuestionAnswer',

    _dataView: 'V_QuestionAnswer',
    _operateTblName: 'Q_QuestionAnswer'

});
