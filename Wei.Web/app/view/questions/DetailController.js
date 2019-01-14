Ext.define('Wei.view.questions.DetailController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.questions_detail',

    // back
    onBackBtnClick: function (t) {
        var controller = t.up('questions').getController();
        controller.setCurrentView('questions_questionbanklist');
    },

    // question bank detail
    onDetailBeforeRender: function (t) {
        var viewmodel = this.getViewModel();
        viewmodel.set('questionBankModel', t._questionbank);
    },


    // question list
    // 题目页面加载
    onQuestionListRender: function (t) {
        var that = this,
            viewmodel = that.getViewModel(),
            grid = t.down('questions_questiongrid'),
            selection = grid.getSelectionModel(),
            qstore = that.getStore('questionlist'),
            qastore = that.getStore('questionanswerlist'),
            questionbankmodel = viewmodel.get('questionBankModel');
        // 加载题目
        qstore.setRemoteFilter(true);
        qstore.on({
            load: function (s, r, suc, oper, eopt) {
                if (suc && r.length > 0) {
                    qstore.setRemoteFilter(false);
                    selection.select(0);
                }
            }
        });
        qstore.filter('questionbank_id', questionbankmodel.getId());
    },
    // question grid
    onQuestionGridSelect: function (t, r, i, eopts) {
        var that = this,
            store = that.getStore('questionanswerlist');
        store.setRemoteFilter(true);
        store.filter('questionid', r.getId());
        store.load({
            callback: function (records, oper, s) {
                store.setRemoteFilter(false);
            }
        });
    },
    onQuestionGridDeselect: function (t, r, i, eopts) {
        var that = this,
            store = that.getStore('questionanswerlist');
        store.setData([]);
    },
    onQuestionAdd: function (t) {
        var that = this,
            grid = t.up('grid'),
            viewmodel = that.getViewModel(),
            store = grid.getStore(),
            questionbankmodel = viewmodel.get('questionBankModel'),
            maxsort = store.max('sort');
        if (!maxsort)
            maxsort = 0;
        var newsort = maxsort + 1;
        store.add({
            questionbank_id: questionbankmodel.getId(),
            sort: newsort,
            qtype: 'Text'
        });
        that.onQuestionEdit(t);
    },
    onQuestionEdit: function (t) {
        var that = this,
            grid = t.up('grid');
        that.onGridBaseMultiRowEditor(grid, false);
    },
    onQuestionDel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        that.onGridBaseMultiRowEditor(grid, false);
        store.remove(selection);
    },
    onQuestionCancel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        // 禁止编辑
        that.onGridBaseEditor(grid, false);
        store.reload();
    },
    onQuestionSave: function (t) {
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
    },
    onQuestionAnswerAdd: function (t) {
        var that = this,
            qgrid = that.getView().down('questions_questiongrid'),
            qselection = qgrid.getSelection(),
            grid = t.up('grid'),
            store = grid.getStore(),
            maxsort = store.max('sort');
        if (!qselection || qselection.length == 0)
            return;
        if (!maxsort)
            maxsort = 0;
        var newsort = maxsort + 1;
        store.add({
            questionid: qselection[0].getId(),
            sort: newsort
        });
        that.onQuestionAnswerEdit(t);
    },
    onQuestionAnswerEdit: function (t) {
        var that = this,
            grid = t.up('grid');
        that.onGridBaseMultiRowEditor(grid, false);
    },
    onQuestionAnswerDel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        that.onGridBaseMultiRowEditor(grid, false);
        store.remove(selection);
    },
    onQuestionAnswerCancel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        // 禁止编辑
        that.onGridBaseEditor(grid, false);
        store.reload();
    },
    onQuestionAnswerSave: function (t) {
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
});
