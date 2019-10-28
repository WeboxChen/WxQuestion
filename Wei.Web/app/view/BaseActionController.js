Ext.define('Wei.view.BaseActionController', {
    extend: 'Wei.view.BaseController',

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
        // 开启单行编辑模式
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
    onDataQuery: function (t) {
        var that = this,
            mainview = that.getView(),
            filterForm = t.up('form'),
            grid = mainview.down('grid'),
            store = grid.getStore(),
            data = filterForm.getValues();

        store.setRemoteFilter(true);
        this.sendFilter(store, data);
    },
    onDataReset: function (t) {
        var that = this,
            filterForm = t.up('form');

        filterForm.reset();
        that.onDataQuery(t);
    }
});
