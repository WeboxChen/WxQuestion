Ext.define('Wei.view.account.UserAttributeViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.account_userattribute',

    data: {
    },

    stores: {
        userattributelist: {
            type: 'account_userattribute'
        }
    }
});
