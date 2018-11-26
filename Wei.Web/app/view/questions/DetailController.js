Ext.define('Wei.view.questions.DetailController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.questions_detail',

    // question bank detail
    onDetailBeforeRender: function (t) {
        var viewmodel = this.getViewModel();
        viewmodel.set('questionBankModel', t._questionbank);
    },

    // question 
    onAdd: function (t) {
        var that = this,
            grid = t.up('grid'),
            viewmodel = that.getViewModel(),
            store = grid.getStore(),
            questionbankmodel = viewmodel.get('questionBankModel'),
            maxsort = store.max('sort');
        if (!maxsort)
            maxsort = 0;
        newsort = maxsort + 1;
        store.add({
            questionbank_id: questionbankmodel.getId(),
            sort: newsort
        });
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

    // 题目页面加载
    onQuestionListRender: function (t) {
        var that = this,
            viewmodel = that.getViewModel(),
            store = that.getStore('questionlist'),
            questionbankmodel = viewmodel.get('questionBankModel');
        store.filter('questionbank_id', questionbankmodel.getId());
        store.load();
    },
    // 题目表格答案类型列加载
    onQuestionGridATypeRenderer: function(v) {
        var store = this.getStore('answertypelist');
        var index = store.find('code', v);
        if (index >= 0)
            return store.getAt(index).get('name');
        return '';
    },
    // 题目表格问题类型列加载
    onQuestionGridQTypeRenderer : function(v, m, r) {
        var store = this.getStore('questiontypelist');
        var index = store.find('code', v);
        if (index >= 0)
            return store.getAt(index).get('name');
        return '';
    }
});
