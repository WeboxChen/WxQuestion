Ext.define('Wei.store.questions.QuestionItem', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.questions_questionitem',
    model: 'Wei.model.questions.QuestionItem',

    _dataView: 'Q_QuestionItem',
    _operateTblName: 'Q_QuestionItem'

});
