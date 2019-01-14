Ext.define('Wei.view.custom.IndexController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.custom',

    // common
    setCurrentView: function (view, params) {
        var contentPanel = this.getView();

        //We skip rendering for the following scenarios:
        // * There is no contentPanel
        // * view xtype is not specified
        // * current view is the same
        if (!contentPanel || view === '' || (contentPanel.down() && contentPanel.down().xtype === view)) {
            return false;
        }

        if (params && params.openWindow) {
            var cfg = Ext.apply({
                xtype: 'common_window',
                items: [
                    Ext.apply({
                        xtype: view
                    }, params.targetCfg)
                ]
            }, params.windowCfg);
            Ext.create(cfg);
        } else {
            Ext.suspendLayouts();
            contentPanel.removeAll(true);
            contentPanel.add(
                Ext.apply({
                    xtype: view
                }, params)
            );
            Ext.resumeLayouts(true);
        }
    },
    init: function () {
        this.setCurrentView('custom_useranswerlist');
    },
    onCustomShow: function () {
        this.setCurrentView('custom_useranswerlist');
    },

    // user answer
    onDataQuery: function (t) {
        var that = this;
        var store = that.getStore('useranswerlist');
        store.load();
    },
    onUserAnswerView: function (t) {
        var that = this,
            grid = t.up('grid'),
            selection = grid.getSelection();
        if (selection.length == 0)
            return;
        var record = selection[0];
        that.setCurrentView('custom_detail', { _useranswer: record });
    },

    // user answer grid
    onUserAnswerDblClick: function (t, r, ele, ri, e, eopts) {
        this.setCurrentView('custom_detail', { _useranswer: r });
    },

    // detail
    onBackBtnClick: function (t) {
        this.setCurrentView('custom_useranswerlist');
    },

    // user answer detail list
    onUserAnswerDetailListRender: function (t) {
        var that = this,
            detailview = t.up('custom_detail'),
            useranswer = detailview._useranswer,
            useranswerdetailgrid = t.down('custom_useranswerdetailgrid'),
            selection = useranswerdetailgrid.getSelectionModel(),
            userdetailstore = that.getStore('useranswerdetaillist');
        userdetailstore.setRemoteFilter(true);
        userdetailstore.on({
            load: function (s, r, suc, oper, eopt) {
                if (suc && r.length > 0) {
                    userdetailstore.setRemoteFilter(false);
                    selection.select(0);
                }
            }
        });
        userdetailstore.filter('useranswer_id', useranswer.getId());
    }
});
