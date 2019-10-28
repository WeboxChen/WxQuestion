Ext.define('Wei.view.setting.WordsSubstitutionViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.setting_wordssubstitution',


    stores: {
        wordssubstitutionlist: {
            type: 'sys_wordssubstitution',
            autoLoad: false
        }
    }
});
