Ext.define('Wei.store.account.User', {
    extend: 'Wei.store.BaseStore',

    alias: 'store.account_user',
    model: 'Wei.model.account.User',

    _dataView: 'V_User',
    _operateTblName: 'A_User'

});
