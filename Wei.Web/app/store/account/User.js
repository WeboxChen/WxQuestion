Ext.define('Wei.store.account.User', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.account_user',
    model: 'Wei.model.account.User',

    _dataView: 'A_User',
    _operateTblName: 'A_User'

});