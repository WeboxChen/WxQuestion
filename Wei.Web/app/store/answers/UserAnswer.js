Ext.define('Wei.store.answers.Useranswer', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.answers_useranswer',
    model: 'Wei.model.answers.UserAnswer',

    _dataView: 'C_UserAnswer',
    _operateTblName: 'C_UserAnswer'

});
