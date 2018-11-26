Ext.define('Wei.store.BaseStore', {
    extend: 'Ext.data.Store',

    alias: 'store.basestore',
    autoLoad: false,
    autoSync: false,
    autoSort: true,
    remoteFilter: false,
    remoteSort: false,
    pageSize: 0,

    proxy: {
        type: 'weiapi',
    },
    _dataView: '',          // 视图名称或表名
    _operateTblName: '',    // 操作表名
    _direction: 'asc',

    // 日志记录用
    _objId: 0,
    _objType: 0,
    _objName: '',

    listeners: {
        beforeload: function (s, operation, eOpts) {
            // 视图数据，写入视图参数和列参数
            var proxy = s.getProxy();

            proxy.setExtraParam('extraView', this._dataView);
            proxy.setExtraParam('direction', this._direction);
        },

        filterchange: function (t, filters, eopts) {
            var fields = t.getModel().getFields();
            Ext.each(filters, function (filter) {
                // 日期筛选,最大日期+1
                var prop = filter.getProperty();
                var operator = filter.getOperator();
                var value = filter.getValue();
                for (var i = 0; i < fields.length; i++) {
                    var f = fields[i];
                    if (f.name == prop && f.type == 'date') {
                        if (operator == "lt" || operator == "<" || operator == "<=") {
                            var nDate = new Date(value);
                            nDate = new Date(nDate.setDate(nDate.getDate() + 1));
                            filter.setValue(nDate.toLocaleDateString());
                        }
                    }
                }
            })
        },

        // 同步操作， 添加同步使用数据
        beforesync: function (o, eopt) {
            // 同步前写入参数
            if (this._operateTblName)
                this.setOperateTblName();

            if (this._objId && this._objType)
                this.setActionRecordParas();
        }
    },
   

    // 
    setOperateTblName: function (paras) {
        var proxy = this.getProxy();
        proxy.setExtraParam("tblname", this._operateTblName);
    },
    setProcExtraParas: function () {
        var proxy = this.getProxy();
        if (this._procParas)
            for (var i in this._procParas)
                proxy.setExtraParam(i, this._procParas[i]);
    },

    setActionRecordParas: function () {
        var proxy = this.getProxy();
        proxy.setExtraParam("_objId", this._objId);
        proxy.setExtraParam("_objType", this._objType);
        proxy.setExtraParam("_objName", this._objName);
    },

    // 封装的简易提交
    ypuSimpleSync: function (options) {
        var that = this;
        if (!options)
            options = {}
        that.sync({
            params: options["params"],
            success: function (b, o) {
                var flag = true;
                for (var i = 0; i < b.operations.length; i++) {
                    var operation = b.operations[i];
                    var response = operation.getResponse().responseText;
                    var obj = JSON.parse(response);

                    if (!obj.success) {
                        flag = false;
                        Ext.Msg.alert('Error', obj.msg);
                    }
                }
                if(flag && options["success"])
                    options["success"]();
                else if(!flag && options["failure"])
                    options["failure"]();
                //var response = b.operations[0].getResponse().responseText;
                //var obj = JSON.parse(response);
                //if (obj && obj.success) {
                //    if (options["success"])
                    //        options["success"]
                //} else {
                //    Ext.Msg.alert('Error', obj.msg);
                //    if (options["failure"])
                //        options["failure"](obj);
                //}
            },
            failure: function (b, o) {
                for (var i = 0; i < b.operations.length; i++) {
                    var error = b.operations[i].getError();
                    if (!error)
                        continue;
                    if (error.response) {
                        var response = JSON.parse(error.response);
                        Ext.Msg.alert('Error' + error['status'], response.Message);
                    } else {
                        Ext.Msg.alert('Error', error['status'] + ': ' + error['statusText']);
                    }
                }

                if (options["failure"])
                    options["failure"](error);
            }
        })
    },
    // 获取属性筛选集合
    getFilterByProperty: function (pname) {
        var that = this,
            filters = that.getFilters();
        return filters.findBy(function (item, key) {
            return item.getProperty() == pname;
        });
    }
});
