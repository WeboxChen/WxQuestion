Ext.define('Wei.view.setting.PropertyViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.setting_property',

    stores: {
        propertystore: {
            type: 'sys_property',
            autoLoad: true
        },

        propertyvaluestore: {
            type: 'sys_propertyvalue',
            remoteFilter: true
        }
    }
});
