Ext.define('Wei.view.account.UserAttributeController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.account_userattribute',

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
    checkData: function (store) {
        var flag = true;
        var message = "";
        store.data.each(function (item, i) {
            var itemname = item.get('name');
            if (store.query('name', itemname).length > 1) {
                message = '【' + itemname + '】代码重复！';
                flag = false;
            }
        });
        if (!flag) {
            this.alertErrorMsg(message);
        }
        return flag;
    },
    onSave: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        if (!that.checkData(store))
            return;

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
