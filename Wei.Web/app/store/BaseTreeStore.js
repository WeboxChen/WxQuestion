Ext.define('Wei.store.BaseTreeStore', {
    extend: 'Ext.data.TreeStore',

    alias: 'store.basetreestore',

    filterer: 'bottomup',           // 筛选默认筛选子项，再到父级节点
    autoLoad: false,
    remoteFilter: false,
    _dataView: '',
    // 存储过程操作数据
    _readProc: '',
    _createProc: '',
    _deleteProc: '',
    _updateProc: '',
    _procParas: {},

    // treestore 参数
    _parentid: '',
    _leaf: '',
    _expanded: '',

    proxy: {
        type: 'weitreeapi',
    },

    root: {
        expanded: true,
        children: []
    },

    listeners: {
        beforeload: function (s, operation, eOpts) {
            // 视图数据，写入视图参数和列参数
            var proxy = this.getProxy();
            if (this._dataView) 
                proxy.setExtraParam('extraView', this._dataView);
            
            if (this._readProc)
                this.setReadProcParas();

            proxy.setExtraParam('F_ParentId', this._parentid);
            proxy.setExtraParam('F_Leaf', this._leaf);
            proxy.setExtraParam('F_Expanded', this._expanded);
        },

        // 同步操作， 添加同步使用数据
        beforesync: function (o, eopt) {
            // 同步前写入参数
            if (o.create) {
                if (this._createProc)
                    this.setCreateProcParas();
            } else if (o.update) {
                if (this._updateProc)
                    this.setUpdateProcParas();
            } else if (o.destroy) {
                if (this._deleteProc)
                    this.setDeleteProcParas();
            }
            if (this._operateTblName)
                this.setOperateTblName();
        }
    },

    // 使用存储过程调用写入默认参数数据
    setReadProcParas: function (paras) {
        var proxy = this.getProxy();
        if (paras) {
            paras.procname = this._readProc;
            proxy.setExtraParams(paras);
        } else {
            proxy.setExtraParams({ procname: this._readProc });
        }
        this.setProcExtraParas();
    },
    setDeleteProcParas: function (paras) {
        var proxy = this.getProxy();
        if (paras) {
            paras.procname = this._deleteProc;
            proxy.setExtraParams(paras);
        } else {
            proxy.setExtraParams({ procname: this._deleteProc });
        }
        this.setProcExtraParas();
    },
    setUpdateProcParas: function (paras) {
        var proxy = this.getProxy();
        if (paras) {
            paras.procname = this._updateProc;
            proxy.setExtraParams(paras);
        } else {
            proxy.setExtraParams({ procname: this._updateProc });
        }
        this.setProcExtraParas();
    },
    setCreateProcParas: function (paras) {
        var proxy = this.getProxy();
        if (paras) {
            paras.procname = this._createProc;
            proxy.setExtraParams(paras);
        } else {
            proxy.setExtraParams({ procname: this._createProc });
        }
        this.setProcExtraParas();
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
    }
});
