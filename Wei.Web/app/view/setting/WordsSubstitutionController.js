Ext.define('Wei.view.setting.WordsSubstitutionController', {
    extend: 'Wei.view.BaseActionController',
    alias: 'controller.setting_wordssubstitution',

    onWordsSubstitutionGridRender: function (t) {
        var that = this,
            viewmodel = that.getViewModel(),
            store = viewmodel.getStore('wordssubstitutionlist');
        //console.log(store)
        if (t._record) {
            store.setRemoteFilter(true);
            store.filter('questionbankid', t._record.getId());
        } else {
            store.load();
        }
    },

    onWordsSubstitutionAdd: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        store.add({ questionbankid: grid._record.getId(), questionbankname: grid._record.get('title') });
        that.onEdit(t);
    }
});
