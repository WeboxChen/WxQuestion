Ext.define('Wei.view.setting.PropertyController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.setting_property',

    onPropertyValueAdd: function (t) {
        var that = this,
            view = that.getView(),
            propertygrid = view.down('setting_propertygrid'),
            propertyselection = propertygrid.getSelection(),
            grid = t.up('grid'),
            store = grid.getStore();
        if (propertyselection.length == 0)
            return;

        store.add({ propertyid: propertyselection[0].getId() });
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

    onPropertySelect: function (t, record, i, eopts) {
        var that = this,
            view = that.getView(),
            pvgrid = view.down('setting_propertyvaluegrid'),
            pvstore = pvgrid.getStore();

        pvstore.filter('propertyid', record.getId());
    }
});
