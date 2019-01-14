Ext.define('Wei.store.custom.UserAnswer', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.custom_useranswer',
    model: 'Wei.model.custom.UserAnswer',

    _dataView: 'V_UserAnswer',
    _operateTblName: 'C_UserAnswer'

});
