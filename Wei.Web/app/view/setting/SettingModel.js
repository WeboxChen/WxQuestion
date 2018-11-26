Ext.define('Wei.view.setting.SettingModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.setting',

    stores: {
        moduletypelist: {
            type: 'setting_moduletype',
            autoLoad: true
        },

        questionbanktaglist: {
            type: 'setting_questionbanktag',
            autoLoad: true
        }
    }
});
