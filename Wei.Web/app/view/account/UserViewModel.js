Ext.define('Wei.view.account.UserViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.account',

    data: {
    },

    stores: {
        userlist: {
            type: 'account_user'
        }
    }
});
