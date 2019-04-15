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
        viewmodel.set('QuestionBank_Id', t._questionbank.getId());
    },

    //question bank info
    onQuestionBankInfoRender: function (t) {
        var that = this,
            viewmodel = that.getViewModel(),
            questionbankmodel = viewmodel.get('questionBankModel');
        t._record = questionbankmodel;
    },
    onQuestionBankInfoSave: function (t) {
        var that = this,
            baseview = t.up('questions'),
            form = t.up('questions_questionbankinfo'),
            baseviewmodel = baseview.getViewModel(),
            store = baseviewmodel.getStore('questionbanklist');
        form.updateRecord();
        store.ypuSimpleSync({

            success: function () {
                store.reload();
            }
        })
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
            qanswerstore = that.getStore('questionanswerlist'),
            qitemstore = that.getStore('questionitemlist');
        if (isNaN(r.getId()))
            return;

        // 加载选项
        qitemstore.setRemoteFilter(true);
        qitemstore.filter('questionid', r.getId());
        qitemstore.load({
            callback: function (records, oper, s) {
                qitemstore.setRemoteFilter(false);
            }
        });

        // 加载答案
        qanswerstore.setRemoteFilter(true);
        qanswerstore.filter('questionid', r.getId());
        qanswerstore.load({
            callback: function (records, oper, s) {
                qanswerstore.setRemoteFilter(false);
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
    onFormAfterrender: function (t) {

    },
    // 导入题目
    onQuestionListImport: function (t, v, eopts) {
        if (v === '')
            return;
        var that = this,
            form = t.up('form'),
            viewmodel = that.getViewModel(),
            questionbankmodel = viewmodel.get('questionBankModel');
        form.submit({
            url: 'api/questionbank/questionbankimport',
            waitMsg: '上传中，请等待...',
            //params: { _objId: questionbankmodel.getId() },
            success: function (form, action) {
                if (!action.result.msg) {
                    that.alertSuccessMsg();
                    var store = that.getStore('questionlist');
                    store.reload();
                } else {
                    that.alertErrorMsg(action.result.msg);
                }
            },
            failure: function (form, action) {
                that.alertErrorMsg(action.result.msg);
            }
        });
    },

    // question item

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

    onQuestionItemAdd: function (t) {
        var that = this,
            qgrid = that.getView().down('questions_questiongrid'),
            qselection = qgrid.getSelection(),
            grid = t.up('grid'),
            store = grid.getStore();
        if (!qselection || qselection.length == 0)
            return;
        store.add({
            questionid: qselection[0].getId()
        });
        that.onQuestionAnswerEdit(t);
    },
    onQuestionItemEdit: function (t) {
        var that = this,
            grid = t.up('grid');
        that.onGridBaseMultiRowEditor(grid, false);
    },
    onQuestionItemDel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        that.onGridBaseMultiRowEditor(grid, false);
        store.remove(selection);
    },
    onQuestionItemCancel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore();
        // 禁止编辑
        that.onGridBaseEditor(grid, false);
        store.reload();
    },
    onQuestionItemSave: function (t) {
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
