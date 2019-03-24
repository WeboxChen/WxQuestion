Ext.define('Wei.view.questions.IndexController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.questions',

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
        this.setCurrentView('questions_questionbanklist');
    },
    onQuestionShow: function () {
        this.setCurrentView('questions_questionbanklist');
    },

    // question bank
    onDataQuery: function (t) {
        var store = that.getStore('questionbanklist');
        store.load();
    },
    onQuestionBankAdd: function (t) {
        var that = this;
        var store = that.getStore('questionbanklist');
        this.alertFormObjWindow('questions_questionbankinfo', '添加题卷', null, {}, function (values) {
            var flag = false;
            store.add(values);
            store.ypuSimpleSync({
                success: function () {
                    flag = true;
                    store.reload();
                },
                error: function () {
                    flag = false;
                }
            });
            return flag;
        });
    },
    onQuestionBankEdit: function (t) {
        var that = this,
            store = that.getStore('questionbanklist'),
            grid = t.up('grid'),
            selected = grid.getSelection();
        if (selected && selected.length == 1) {
            var record = selected[0];
            this.alertFormObjWindow('questions_questionbankinfo', '修改题卷', record, { }, function (values) {
                store.ypuSimpleSync({
                    success: function () {
                        store.reload();
                    }
                });
                return true;
            });
        }
        
    },
    onQuestionBankView: function (t) {
        var that = this,
            grid = t.up('grid'),
            selection = grid.getSelection();
        if (selection.length == 0)
            return;
        var record = selection[0];
        that.setCurrentView('questions_detail', { _questionbank: record });
    },
    onQuestionBankDel: function (t) {
        var that = this,
            grid = t.up('grid'),
            store = grid.getStore(),
            selection = grid.getSelection();
        that.confirmMsg('是否删除选中数据？', function () {
            store.remove(selection);
            store.ypuSimpleSync({
                success: function () {
                    store.reload();
                }
            });
        })
    },

    // questionBank grid
    onQuestionBankGridDblClick: function (t, r, ele, ri, e, eopts) {
        var that = this;
        that.setCurrentView('questions_detail', { _questionbank: r });
    }
    
});
