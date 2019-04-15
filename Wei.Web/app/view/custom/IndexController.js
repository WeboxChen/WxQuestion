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

    onAdd: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        store.add({});
        that.onEdit(t);
    },
    onEdit: function (t) {
        var that = this,
            grid = t.up('grid');
        that.onGridBaseMultiRowEditor(grid, false);
    },
    onMultiEdit: function (t) {
        var that = this,
            grid = t.up('grid');
        that.onGridBaseMultiRowEditor(grid, true);
    },
    onDel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        that.onGridBaseMultiRowEditor(grid, false);
        store.remove(selection);
    },
    onCancel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        // 禁止编辑
        that.onGridBaseEditor(grid, false);
        store.reload();
    },
    onSave: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();

        var mrecords = store.getModifiedRecords();
        var rrecords = store.getRemovedRecords();

        if (mrecords.length > 0 || rrecords.length > 0) {
            store.ypuSimpleSync({
                success: function () {
                    that.onGridBaseEditor(grid, false);
                    store.reload();
                }
            });
        } else {
            that.onGridBaseEditor(grid, false);
        }
    },

    // user answer
    onDataQuery: function (t) {
        var that = this,
            view = that.getView().down('custom_useranswerlist'),
            grid = view.down('gird'),
            filterForm = view.down('#useranswer_form_filter'),
            data = filterForm.getValues(),
            store = that.getStore('useranswerlist');
        //var store = that.getStore('useranswerlist');
        //store.load();
        store.setRemoteFilter(true);
        that.sendFilter(store, data);
    },
    onUserAnswerView: function (t) {
        var that = this,
            grid = t.up('grid'),
            selection = grid.getSelection();
        if (selection.length == 0) {
            that.alertErrorMsg("请选中一条记录！");
            return;
        }
        var record = selection[0];
        that.setCurrentView('custom_detail', { _useranswer: record });
    },
    onUserAnswerDiscard: function (t) {
        var that = this,
            view = that.getView().down('custom_useranswerlist'),
            grid = view.down('custom_useranswergrid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        if (selection.length == 0) {
            that.alertErrorMsg("请选中一条记录！");
            return;
        }
        var ids = [];
        selection.forEach(function (item) {
            ids.push(item.getId());
        });
        that.postJsonSync('useranswer/discard', {
                ids: ids
            }, function (obj, response, eopts) {
                store.reload();
            }, function (response, eopts) {
                store.reload();
            });
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
                    // selection.select(0);
                }
            }
        });
        userdetailstore.filter('useranswer_id', useranswer.getId());
    },
    onAnswerViocePlay: function (t) {
        var gpitem = t.up('buttongroup');
        var path = gpitem._rowContext.record.get('voicepath');

        if (path) {
            RongIMLib.RongIMVoice.init();
            RongIMLib.RongIMVoice.play(path);
        }
        //console.log(path);
    },
    onAnswerVioceStop: function (t) {
        //console.log(t.data);
        RongIMLib.RongIMVoice.stop();
    }
});
