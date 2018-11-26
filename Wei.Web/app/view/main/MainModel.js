Ext.define('Wei.view.main.MainModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.main',

    data: {
        currentView: null
    },

    stores: {
        navigationTree: {
            type: 'NavigationTree'
        },

        statestore: {
            type: 'store',
            data: [
                { id: 0, name: '启用' },
                { id: -1, name: '禁用' }
            ]
        },

        moduletypelist: {
            type: 'setting_moduletype',
            _autoload: true
        },

        questionbanktaglist: {
            type: 'setting_questionbanktag',
            _autoload: true
        },
    }
});
