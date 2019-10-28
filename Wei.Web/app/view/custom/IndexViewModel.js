Ext.define('Wei.view.custom.IndexViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.custom',


    stores: {
        useranswerlist: {
            type: 'custom_useranswer',
            autoLoad: true
        },

        useranswerdetaillist: {
            type: 'custom_useranswerdetail',
            remoteFilter: true
        }
    }
});
