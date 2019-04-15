Ext.define('Wei.view.BaseController', {
    extend: 'Ext.app.ViewController',

    // 获取配置信息
    getBaseConfig: function () {
        var data = Ext.create('Wei.data.base.BaseConfig');
        return data.data[0];
    },
    // 加载基础数据
    loadBaseData: function () {
        var that = this;
        Ext.ns('wei.data');
        wei.data.current = {};
        wei.data.baseData = {};
        // 加载全局配置信息
        wei.data.global = that.getBaseConfig();
        // 判断用户登录

        var user = that.GetCurrentUser();
        if (user) {
            that.mainModelLoad();
        }
        that.SetCurrentUserName();
    },
    
    mainModelLoad: function () {
        var that = this,
            mainview = Ext.getCmp('mainView'),
            mainviewmodel = mainview.getViewModel();
        for (var aa in mainviewmodel.storeInfo) {
            var store = mainviewmodel.storeInfo[aa];
            if (store._autoload) {
                store.load();
            }
        }
    },
    // 加载左侧模块信息
    loadModules: function (id) {
        var that = this,
            mainview = Ext.getCmp('mainView'),
            refs = mainview.getReferences(),
            navigationList = refs.navigationTreeList,
            store = navigationList.getStore(),
            user = that.GetCurrentUser(),
            modules = user.ModuleWithChildren;

        //从本地读数据
        if (!id) {
            if (modules.length > 0) {
                id = modules[0].Children[0].Item.Url;
            }
            else {
                id = 'login';
                that.alertErrorMsg('无任何操作权限！');
            }
        }
        var data = [];
        
        Ext.each(modules, function (module, index) {
            var childrens = module.Children;
            var menu = {
                iconCls: 'fa ' + module.Item.IconName,
                viewType: module.Item.Url,
                text: module.Item.Name,
                expanded: false
            };
            var cmenus = [];
            if (childrens && childrens.length > 0) {
                Ext.each(childrens, function (cmodule) {
                    if (cmodule.Item.Url.length < 2)
                        return;
                    if (cmodule.Item.Url == id)
                        menu['expanded'] = true;
                    cmenus.push({
                        leaf: 1,
                        iconCls: 'fa ' + cmodule.Item.IconName,
                        viewType: cmodule.Item.Url,
                        text: cmodule.Item.Name
                    });
                });
            }
            menu.children = cmenus;
            menu.leaf = cmenus.length == 0;
            data.push(menu);
        });
        if (data.length > 0) {
            var root = store.getRoot();
            root.removeAll();
            root.appendChild(data);
        }
        var record = root.findChild('viewType', id, true);
        if (record) {
            navigationList.setSelection(record);
        }
    },
    // 加载基础登录模块
    loadLoginModules: function(){
        var mainview = Ext.getCmp('mainView'),
            refs = mainview.getReferences(),
            navigationList = refs.navigationTreeList,
            store = navigationList.getStore();
        var root = store.getRoot();
        root.insertChild(0, {
            text: '登录',
            iconCls: 'x-fa fa-edit',
            viewType: 'login',
            leaf: true
        });
    },
    checkModules: function () {
        var mainview = Ext.getCmp('mainView'),
            refs = mainview.getReferences(),
            navigationList = refs.navigationTreeList,
            store = navigationList.getStore();
        var root = store.getRoot();
        if (root.childNodes.length == 1 && root.childNodes[0].get('viewType') == 'login')
            return false;
        return true;
    },

    // 获取设置基础Model
    //getSettingModel: function () {
    //    if (!wei.data['SettingModel']) {
    //        wei.data['SettingModel'] = Ext.create("Wei.view.setting.SettingModel");
    //    }
    //    return wei.data['SettingModel'];
    //},

    // 加载流程工序
    loadProcess: function () {
        var that = this,
            mainview = Ext.getCmp('mainView'),
            mainviewmodel = mainview.getViewModel(),
            processstore = mainviewmodel.getStore('processlist'),
            processstepsstore = mainviewmodel.getStore('processstepslist');
        processstepsstore.on({
            load: function (s, records, sucessful, operation) {
                if (!sucessful)
                    return;
                wei.data.process = new (function () {
                    var me = this,
                        _pstore = processstore,
                        _psstore = processstepsstore;

                    me.getProcess = function (processid) {
                        return _pstore.getById(processid);
                    }
                    // 获取流程对象
                    me.getProcessObj = function (processid) {
                        return new (function () {
                            var pme = this,
                                _precord = me.getProcess(processid),
                                _psrecords = _psstore.query('processid', processid);
                            pme.getProcess = function () {
                                return _precord;
                            }
                            pme.getSteps = function () {
                                return _psrecords;
                            }
                            pme.getStep = function (code) {
                                return _psrecords.find('code', code);
                            }
                            pme.getFirst = function () {
                                return _psrecords.find('sort', 1);
                            }
                        });
                    }
                    me.getProcessStep = function (processid, pscode) {
                        return _psstore.queryBy(function (r) {
                            return r.get('processid') == processid && r.get('code') == pscode;
                        }).first();
                    }
                    me.getProcessFirst = function (processid) {
                        return _psstore.queryBy(function (r) {
                            return r.get('processid') == processid && r.get('order') == 1;
                        }).first();
                    }
                });
            }
        });
        processstepsstore.load();
    },
    // 刷新订单状态
    refushOrderStatusButton: function (grid, processid, code) {
        // hide main processgroup & disable subprocess
        var that = this,
            cmps = grid.query('[_type="processMain"]'),
            subcmps = grid.query('[_type="processSub"]');

        Ext.each(subcmps, function (item) {
            item.disable();
        });
        Ext.each(cmps, function (item) {
            item.disable();
        });

        var oProcessObj = wei.data.processes.getProcessOrderObj(processid);
        var record = oProcessObj.getRecord(code);
        //if (!record) {
        //    var fProcessObj = wei.data.processes.getProcessOrderObj(2);
        //    record = fProcessObj.getRecord(code);
        //}

        //var enableds = processorderObj.getEnables(code);
        // 工单流程走完， 显示撤回生产
        if (record) {
            var enableds = record['bd_ad'].split('|');
            if (code == 'DD.DD' && processid == 1)
                enableds = 'FP.CH';

            Ext.each(enableds, function (item) {
                if (item)
                    that.enableMenu(grid, item);
            });
        }
    },
    enableMenu: function (grid, code) {
        var cmp = grid.down('[_code="' + code + '"]');
        if (cmp) {
            var mcmp = cmp.up('splitbutton');
            if (mcmp && mcmp.isDisabled())
                mcmp.enable();
            cmp.enable();
        }
    },
    disableMenu: function (grid, code) {
        var status = grid.down('[_code="' + code + '"]');
        if (status)
            status.disable();
    },
    executeProcess: function (record, type, code, fn1, fn2) {
        var that = this,
            objid = record.getId(),
            processid = record.get('processid'),
            processobj = wei.data.process.getProcessObj(processid),
            step = processobj.getStep(code),
            procname = step.get('executeproc');

        var user = that.GetCurrentUser();
        that.executeProcedureOutMsg(procname, {
            objId: objid,
            type: type,
            code: record.get('pstatus'),
            ncode: code,
            operatorId: user.Id,
            operator: user.UserName,
            description: ''
        }, fn1, fn2);
    },

    // 按钮显示
    displayBtnFunc: function (cmp) {
        var module = wei.data.openauth['modules'][cmp.xtype];
        if (module && module.elements) {
            // 显示按钮
            for (var key in module.elements) {
                var element = module.elements[key];
                var elecmp = cmp.down('[wei_type="' + key + '"]');
                if (elecmp) {
                    if (element.script == "show")
                        elecmp.show();
                    if (element.script == "enable")
                        elecmp.enable();
                }
            }
        }
    },
    displayChildrenModuleByTab: function (cmp) {
        var module = wei.data.openauth['modules'][cmp.xtype];

        // 显示子模块
        for (var key in module.childrens) {
            var cmodule = module.childrens[key];
            cmp.add({
                xtype: cmodule.url,
                title: cmodule.name
            });
        }
        cmp.setActiveItem(0);
    },
    disableBtnByYPuBtnType: function (base, typename) {
        var btns = base.query('[wei_btn_type="' + typename + '"]');
        Ext.each(btns, function (item) {
            item.disable();
        })
    },
    // 显示管理员项
    showDirectorItem: function (t) {
        var category = this.GetSourceCategory('角色资源');
        var resource = this.GetUserResource(category, 'U_Director');
        if (resource) {
            var comps = t.query('[_adminitem=true]');
            Ext.each(comps, function (item) {
                item.show();
            })
        }
    },
    // 显示管理员项
    showAdminItem: function (t) {
        var category = this.GetSourceCategory('角色资源');
        var resource = this.GetUserResource(category, 'U_Admin');
        if (resource) {
            var comps = t.query('[_adminitem=true]');
            Ext.each(comps, function (item) {
                item.show();
            })
        }
    },
    // 获取权限对象  写入内存中  wei.data.openauth[key]
    authDataSerial: function(obj){
        var that = this;
        // 以对象存储的模块
        wei.data.openauth = {};
        {   // 模块权限
            var modulelist = {};
            Ext.each(obj.Modules, function (i) {
                var key = (i.ParentId != null && i.ParentId.length > 0) ? i.Url : i.Id;
                modulelist[key] = that.getAuthModuleObj(i);
            });
            // 写入子节点，父节点
            for (var i in modulelist) {
                var module = modulelist[i];
                var parentid = module.parentId;
                if (parentid && parentid.length > 0) {
                    for (var j in modulelist) {
                        var cmodule = modulelist[j];
                        if (parentid == cmodule.id) {
                            module.parent = cmodule;
                            if (!cmodule['childrens'])
                                cmodule['childrens'] = {};
                            cmodule['childrens'][module.url] = module;
                            break;
                        }
                    }
                }
            }
            wei.data.openauth['modules'] = modulelist;
        }
        {   // 资源权限
            var resourcelist = {};
            var resourceCategory = {
                '9379266a-1b5f-4da5-b2ca-354b7956c73a': 'Factory',
                'fa37b322-508b-47ce-b29f-b1dacb82637a': 'Confidential'
            }
            Ext.each(obj.Resources, function (i) {
                var ckey = resourceCategory[i.CategoryId];
                var key = i.Key;
                if (!ckey) ckey = 'Default';
                if (!resourcelist[ckey]) resourcelist[ckey] = {}
                resourcelist[ckey][key] = {
                    Key: key, Id: i.Id, Name: i.Name,
                    SortNo: i.SortNo, Status: i.Status,
                    CascadeId: i.CascadeId, CategoryId: i.CategoryId,
                    ParentId: i.ParentId, Description: i.Description
                };
            });
            wei.data.openauth['resources'] = resourcelist;
        }
    },
    checkButtonAuth: function (cmpxtype, btnxtype) {
        if (wei.data.openauth && wei.data.openauth['modules']) {
            var module = wei.data.openauth['modules'][cmpxtype];
            if (module && module.elements) {
                var button = module.elements[btnxtype];
                if (button) 
                    return true;
            }
        }
        return false;
    },
    // 获取基础模块权限对象，  模块下按钮权限对象
    getAuthModuleObj: function(module){
        var obj = {
            checked: module.Checked,
            iconName: module.IconName,
            id: module.Id,
            name: module.Name,
            parentId: module.ParentId,
            url: module.Url
        }
        var elements = {};
        Ext.each(module.Elements, function (i) {
            elements[i.DomId] = {
                attr: i.Attr,
                class: i.Class,
                domId: i.DomId,
                icon: i.Icon,
                id: i.Id,
                moduleId: i.ModuleId,
                name: i.Name,
                remark: i.Remark,
                script: i.Script,
                sort: i.Sort,
                type: i.Type
            };
        });
        obj.elements = elements;
        return obj;
    },

    // form筛选组合条件
    sendFilter: function (store, vals) {
        store.getFilters().removeAll();
        var operator = "";
        var property = "";
        var filters = [];
        var newActiveFilter = null;
        for (var x in vals) {
            if (vals[x] === '') continue;
            var params = x.split(".");
            if (params.length > 1) {
                property = params[0];
                operator = params[1];
            } else {
                property = params[0];
                operator = "=";
            }
            newActiveFilter = new Ext.util.Filter({
                id: 'filter' + x,
                operator: operator,
                property: property,
                value: vals[x],
                convert: function (b) {
                    var a = b;
                    var temp = Ext.Date.parse(b, "Y-m-d", true);
                    if (temp) a = Ext.Date.clearTime(temp, true).getTime();
                    return a;
                }
            });
            filters.push(newActiveFilter);
        }
        store.setFilters(filters);
        return store;
    },
    // form绑定record后保存
    updateFormCheckboxRecord: function (form, record) {
        var checkboxcmps = form.query('checkbox');
        var values = form.getValues();
        Ext.each(checkboxcmps, function (item) {
            var name = item.name;
            if (name.length == 5 && name.indexOf('_') == 2 && !values[name])
                record.set(item.name, 0);
        });
    },

    enableGridPlugin: function (grid, pname) {
        var plugin = grid.findPlugin(pname);
        var columns = grid.getColumns();
        if (plugin) {
            plugin.enable();
            Ext.each(columns, function (col) {
                if (col._isEdit && col.isDisabled()) {
                    col.enable();
                }
            });
            grid._editable = 1;
        }
        return plugin;
    },
    disableGridPlugin: function(grid, pname){
        var plugin = grid.findPlugin(pname);
        var columns = grid.getColumns();
        if (plugin) {
            plugin.disable();
            Ext.each(columns, function (col) {
                if (col._isEdit) {
                    col.disable();
                }
            });
            grid._editable = 0;
        }
        return plugin;
    },
    onGridBaseEditor: function (grid, n) {
        var plugin = grid.getPlugin('gridEditor');
        var columns = grid.getColumns();
        if (!plugin) {
            grid.addPlugin({
                ptype: 'cellediting',
                clicksToEdit: 1,
                id: 'gridEditor'
            });
            plugin = grid.getPlugin('gridEditor');
        }
        if (n) {
            this.enableGridPlugin(grid, 'cellediting');
        } else {
            this.disableGridPlugin(grid, 'cellediting');
        }
    },
    onGridBaseMultiRowEditor: function (grid, n) {
        var pname = 'cellediting',
            plugin = grid.findPlugin(pname);
        if (!plugin) {
            grid.addPlugin({
                ptype: 'cellediting',
                clicksToEdit: 1,
                id: 'gridEditor'
            });
            plugin = grid.getPlugin('gridEditor');
        }
        this.enableGridPlugin(grid, pname);
        var selectionmodel = grid.getSelectionModel();
        if (n) {
            // 多选模式
            selectionmodel.setSelectionMode('MULTI');
            // 绑定同步修改事件
            plugin.on({
                edit: 'onChangeMultiRowData'
            })
        } else {
            selectionmodel.deselectAll();
            // 单选模式
            selectionmodel.setSelectionMode('SINGLE');
            // 解绑同步时间
            plugin.un({
                edit: 'onChangeMultiRowData'
            })
        }
    },
    onChangeMultiRowData: function (editor, context, e) {
        var grid = editor.getCmp();
        var selectionmodel = grid.getSelectionModel();
        var selected = selectionmodel.getSelected();

        if (selected.length > 0) {
            for (var i in selected.items) {
                var r = selected.items[i];
                r.set(context.field, context.value);
            }
        }
    },

    // 获取当前用户 
    GetCurrentUser: function () {
        var store = new Ext.util.LocalStorage({ id: 'Wei' });
        var permission = store.getItem("Permission");
        if (permission) {
            var user = Ext.util.Base64.decode(permission);
            return JSON.parse(user);
        }
        return null;
    },
    IsClientUser: function () {
        var user = this.GetCurrentUser();
        return user && user.WebRole == "Client";
    },
    removeUserPermission: function () {
        var store = new Ext.util.LocalStorage({ id: 'Wei' });
        store.removeItem('Permission');
    },
    GetUserResource: function(category, resource){
        var that = this,
            user = that.GetCurrentUser();
        var result;
        for (var i = 0; i < user.Resources.length; i++) {
            var item = user.Resources[i];
            if (item.CategoryId.toLowerCase() != category.toLowerCase()
                || resource && resource.toLowerCase() != item.Key.toLowerCase())
                continue;
            result = item;
            break;
        }
        return result;
    },
    // 设置当前显示用户
    SetCurrentUserName: function () {
        var user = this.GetCurrentUser();
        if (user && user.UserName)
            Ext.getCmp('head_username').setText(user.UserName);
    },
    GetSourceCategory: function (name) {
        var data = Ext.create('Wei.data.base.SourceCategory');
        var gid = '';
        Ext.each(data.data, function (item) {
            if (item.Name == name)
                gid = item.Id;
        });
        return gid.toLowerCase();
    },


    // 根据records获取原型数据 【多层数据转二维数据】
    getDataByRoot: function (root) {
        var that = this;
        var arr = [];
        Ext.each(root.childNodes, function (treenode) {
            arr.push(that.getDataByTreeNode(treenode));
        });
        return arr;
    },
    // 根据record获取原型数据 【多层数据转二维数据】
    getDataByTreeNode: function (treenode) {
        var obj = treenode.getData();
        if (treenode.childNodes && treenode.childNodes.length > 0)
            obj['children'] = this.getDataByChildrenNodes(treenode.childNodes);
        return obj;
    },
    getDataByChildrenNodes: function (childrennodes) {
        var that = this;
        var arr = [];
        Ext.each(childrennodes, function (treenode) {
            arr.push(that.getDataByTreeNode(treenode));
        });
        return arr;
    },

    // 统一提示窗口
    alertErrorMsg: function (context) {
        Ext.Msg.alert('错误', context);
    },
    alertWarningMsg: function (context) {
        Ext.Msg.alert('警告', context);
    },
    alertPromptMsg: function (context) {
        Ext.Msg.alert('提示', context);
    },
    alertSuccessMsg: function (context) {
        Ext.Msg.alert('成功', context);
    },
    confirmMsg: function (context, fn1, fn2) {
        Ext.Msg.confirm('警告', context, function (v, o, e) {
            if (v == 'yes' && fn1)
                fn1(o, e);
            else if(fn2)
                fn2(o, e);
        });
    },
    waitFn: function(fn, title){
        if (!title) title = '执行中...';
        var winmsg = Ext.Msg.wait(title);
        fn(winmsg);
    },
    closeWaitWin: function(win){
        if (win) win.close();
    },
    // requestObj 不包含 callback、 success、 failure
    waitWinRequest: function (requestObj, fn1, fn2, msg) {
        var that = this;
        if (!msg) msg = '执行中...';

        var winmsg = Ext.Msg.wait(msg);
        requestObj['success'] = function (response, eopts) {
            winmsg.close();
            var json = JSON.parse(response.responseText);
            if (json.success) {
                if (fn1)
                    fn1(json.obj, response, eopts);
                return;
            }
            that.alertErrorMsg(json.msg);
            if (fn2)
                fn2(response, eopts);
        };
        requestObj['failure'] = function (response, eopts) {
            winmsg.close();
            if (response.status === 401) return;
            that.alertErrorMsg(response.status + ': ' + response.statusText);
            if (fn2)
                fn2(response, eopts);
        };
        Ext.Ajax.request(requestObj);
    },
    requestNonWaitWin: function (requestObj, fn1, fn2, msg) {
        var that = this;
        if (!msg) msg = '执行中...';

        requestObj['success'] = function (response, eopts) {
            var json = JSON.parse(response.responseText);
            if (json.success) {
                if (fn1)
                    fn1(json.obj, response, eopts);
                return;
            }
            that.alertErrorMsg(json.msg);
            if (fn2)
                fn2(response, eopts);
        };
        requestObj['failure'] = function (response, eopts) {
            if (response.status === 401) return;
            that.alertErrorMsg(response.status + ': ' + response.statusText);
            if (fn2)
                fn2(response, eopts);
        };
        Ext.Ajax.request(requestObj);
    },
    alertWindow: function (title, ctrObj, fn1) {
        var that = this;
        var cfg = {
            xtype: 'common_window',
            width: '80%',
            height: '80%',
            header: {
                title: title
            },
            items: [
                ctrObj
            ],
            bbar: [
                '->',
                {
                    xtype: 'button',
                    text: '确定',
                    handler: function (t) {
                        if (fn1) {
                            fn1(t);
                        }
                    }
                },
                {
                    xtype: 'button',
                    text: '关闭',
                    handler: function (t) {
                        t.up('window').close();
                    }
                }
            ]
        };
        Ext.create(cfg);
    },
    // 传入数组，弹出窗口   
    // data = [{name:'', value:''}, {name:'', value:''}]
    // fn 关闭窗体需要回调的代码
    alertComboWindow: function (data, fn) {
        var that = this;
        var store = Ext.create('Ext.data.Store', {
            fields: ['name', 'value'],
            data: data
        });
        var cfg = {
            xtype: 'common_window',
            width: 220,
            height: 82,
            header: false,
            _sizePrecent: 0,
            items: [
                {
                    xtype: 'form',
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'combobox',
                            hideLabel: true,
                            valueField: 'value',
                            displayField: 'name',
                            value: '请选择--',
                            store: store,
                            editable: false
                        }
                    ],
                    bbar: [
                        '->',
                        {
                            xtype: 'button',
                            text: '确定',
                            handler: function (t) {
                                if (!wei.data.redirect)
                                    wei.data.redirect = {};
                                var form = t.up('form');
                                var combo = form.down('combobox');
                                var value = combo.getValue();
                                if (value !== '请选择--')
                                    wei.data.redirect['comboVal'] = value;
                                t.up('window').close();
                            }
                        },
                        {
                            xtype: 'button',
                            text: '取消',
                            handler: function (t) {
                                t.up('window').close();
                            }
                        }
                    ]
                }
            ]
        };
        var win = Ext.create(cfg);
        win.on({
            close: function (t) {
                fn();
            },
            scope: that
        });
    },
    alertFormWindowSimple: function (title, fieldCmps, confirmFn) {
        this.alertFormWindow(title, null, null, fieldCmps, confirmFn);
    },
    // 传入form模型，弹出form窗口
    alertFormWindow: function (title, winObj, formObj, fieldCmps, confirmFn) {
        var that = this;
        var cfg = {
            xtype: 'common_window',
            width: '50%', // 220,
            height: '50%', // 82,
            header: {
                title: title
            },
            _sizePrecent: 0,
            items: {
                xtype: 'form',
                bodyPadding: 12,
                scrollable: true,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                defaults: {
                    xtype: 'textfield',
                    labelAlign: 'right'
                },
                items: fieldCmps
            },
            bbar: [
                '->', {
                    xtype: 'button',
                    text: '确定',
                    handler: function (t) {
                        var form = t.up('window').down('form');
                        var record = form.getRecord();
                        var flag = true;
                        if (record) {
                            form.updateRecord();
                            flag = confirmFn(form.getRecord());
                        } else {
                            flag = confirmFn(form.getValues());
                        }
                        if (!flag)
                            return;
                        t.up('window').close();
                    }
                }, {
                    xtype: 'button',
                    text: '取消',
                    handler: function (t) {
                        t.up('window').close();
                    }
                }
            ]
        };
        if (!title) cfg.header = false;
        if (winObj) {
            for (var i in winObj)
                cfg[i] = winObj[i];
        } 
        if (formObj) {
            for (var j in formObj)
                cfg.items[j] = formObj[j];
        } 
        var win = Ext.create(cfg);
    },
    alertFormObjWindow: function (fxtype, title, record, winObj, confirmFn) {
        var that = this;
        var cfg = {
            xtype: 'common_window',
            width: '80%',
            height: '80%',
            header: {
                title: title
            },
            _sizePrecent: 0,
            items: {
                xtype: fxtype,
                _record: record
            },
            tbar: [
                {
                    xtype: 'button',
                    text: '保 存',
                    handler: function (t) {
                        var form = t.up('window').down(fxtype);
                        var record = form.getRecord();
                        var flag = true;
                        if (record) {
                            form.updateRecord();
                            flag = confirmFn(form.getRecord());
                        } else {
                            flag = confirmFn(form.getValues());
                        }
                        if (!flag)
                            return;
                        t.up('window').close();
                    }
                }, {
                    xtype: 'button',
                    text: '取 消',
                    handler: function (t) {
                        t.up('window').close();
                    }
                }
            ]
        };
        if (!title) cfg.header = false;
        if (winObj) {
            for (var i in winObj)
                cfg[i] = winObj[i];
        }
        var win = Ext.create(cfg);
    },
    openFrame: function (url, title, wincfg, framecfg) {
        var frame = {
            xtype: 'uxiframe',
            src: url
        };
        if (framecfg)
            for (var i in framecfg)
                frame[i] = framecfg[i];
        var win = {
            title: title,
            items: frame
        };
        if (wincfg)
            for (var k in wincfg)
                win[k] = wincfg[k];
        if (wincfg['width'] || wincfg['height'])
            win['_sizePrecent'] = 0;
        return Ext.create('Wei.view.common.Window', win);
    },
    showReport: function (src, paras, title) {
        var frame = Ext.create('Ext.ux.IFrame', {
            frameName: title,
            src: wei.data.global.reportServer + src + paras
        });
        var win = Ext.create('Wei.view.common.Window', {
            title: title,
            items: frame
        });
    },
    // number 是否为小数 
    // true 小数
    // false 整数
    isFloatNumber: function (number) {
        return number.toString().indexOf('.') > 0;
    },

    getUserList: function () {
        var userlist = [];
        var data = Ext.create('Wei.data.base.SplitUser');
        Ext.each(data.data, function (item) {
            userlist.push(item);
        });
        return userlist;
    },

    // post请求封装
    // json 数据对象
    postJson: function (url, jsonData, fn1, fn2, nonWaitWin) {
        var that = this;
        if (nonWaitWin) {
            that.requestNonWaitWin({
                url: url,
                jsonData: jsonData,
                method: 'POST'
            }, fn1, fn2);
        } else {
            that.waitWinRequest({
                url: url,
                jsonData: jsonData,
                method: 'POST'
            }, fn1, fn2);
        }
    },
    postJsonSync: function (url, jsonData, fn1, fn2, nonWaitWin, nonEncrypt) {
        var that = this;
        if (nonWaitWin) {
            that.requestNonWaitWin({
                url: url,
                jsonData: jsonData,
                method: 'POST',
                async: false,
                _noencrypt: nonEncrypt
            }, fn1, fn2);
        } else {
            that.waitWinRequest({
                url: url,
                jsonData: jsonData,
                method: 'POST',
                async: false,
                _noencrypt: nonEncrypt
            }, fn1, fn2);
        }
    },
    // post请求
    // url参数数据
    postParams: function (url, params, fn1, fn2, nonWaitWin, nonEncrypt) {
        var that = this;
        if (nonWaitWin) {
            that.requestNonWaitWin({
                url: url,
                params: params,
                method: 'POST',
                _noencrypt: nonEncrypt
            }, fn1, fn2);
        } else {
            that.waitWinRequest({
                url: url,
                params: params,
                method: 'POST',
                _noencrypt: nonEncrypt
            }, fn1, fn2);
        }
    },
    postData: function (url, params, jsonData, fn1, fn2, nonWaitWin, nonEncrypt) {
        var that = this;
        if (nonWaitWin) {
            that.requestNonWaitWin({
                url: url,
                params: params,
                jsonData: jsonData,
                method: 'POST',
                _noencrypt: nonEncrypt
            }, fn1, fn2);
        } else {
            that.waitWinRequest({
                url: url,
                params: params,
                jsonData: jsonData,
                method: 'POST',
                _noencrypt: nonEncrypt
            }, fn1, fn2);
        }
    },
    // 执行存储过程方法
    executeProcedure: function (procname, params, fn1, fn2) {
        this.postJson('api/common/ExecuteByProcedure?procname=' + procname, params, fn1, fn2);
    },
    // 执行带返回参数的存储过程方法
    executeProcedureOutMsg: function (procname, params, fn1, fn2) {
        this.postJson('api/common/ExecuteByProcedureOutMsg?procname=' + procname, params, fn1, fn2);
    },

    getProductionTypeNameById: function (ptid) {
        var productiontype;
        var ptdata = wei.data.baseData.productionType;
        for (var i = 0; i < ptdata.length; i++) {
            if (ptdata[i].Id == ptid) {
                productiontype = ptdata[i].Name;
                break;
            }
        }
        return productiontype;
    },
    // 延迟执行
    setTimeOut: function (fn, waitms, scope) {
        if (!scope) scope = this;
        // 创建任务管理器
        var taskmgr = new Ext.util.TaskRunner();
        var task1 = {
            run: fn,
            interval: waitms,
            repeat: 1,
            scope: scope,
            fireOnStart: false
        };
        // 执行任务
        taskmgr.start(task1);
    },
    // 延迟执行，满足条件，执行方法
    // 例如： conditionfn = function(){ return store.isLoaded(); }
    //        fn = function() { console.log('execute'); }
    delayFn: function (conditionfn, fn, scope) {
        if (!scope)
            scope = this;
        var taskmgr = new Ext.util.TaskRunner();
        var task1;
        var taskconfig = {
            run: function () {
                if (conditionfn()) {
                    fn();
                    taskmgr.stop(task1);
                }
            },
            interval: 200,
            scope: scope,
            fireOnStart: true
        };
        // 执行任务
        task1 = taskmgr.start(taskconfig);
    }
});
