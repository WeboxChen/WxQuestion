Ext.define('Wei.store.questions.Question', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.questions_question',
    model: 'Wei.model.questions.Question',

    _dataView: 'Q_Question',
    _operateTblName: 'Q_Question', 

    listeners: {
        beforeload: function (s, o) {
            if (!s.getFilterByProperty('questionbank_id')) {
                return false;
            }
        }
    }

});
