Ext.define('Wei.view.account.UserController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.account',

    onAccountGridRender: function (t) {
        var that = this,
            store = that.getStore('userlist');
        store.load();
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
    }
});
