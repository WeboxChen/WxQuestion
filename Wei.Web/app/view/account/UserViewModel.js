Ext.define('Wei.view.account.UserViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.account',

    //data: {
    //    marrieddata: [
    //        { name: '未婚', value: false },
    //        { name: '已婚', value: true }
    //    ]
    //},

    stores: {
        userlist: {
            type: 'account_user'
        },

        marriedstore: {
            fields: [
                { name: 'text', type: 'string' },
                { name: 'value', type: 'bool' }
            ],
            data: [
                { name: '未婚', value: false },
                { name: '已婚', value: true }
            ]
        }
    }
});
