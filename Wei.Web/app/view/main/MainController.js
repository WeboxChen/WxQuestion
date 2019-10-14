Ext.define('Wei.view.main.MainController', {
    extend: 'Wei.view.BaseController',
    alias: 'controller.main',

    listen : {
        controller : {
            '#' : {
                unmatchedroute: 'onRouteChange'
            }
        }
    },

    routes: {
        ':node': 'onRouteChange'
    },
    init: function () {
        this.loadBaseData();
    },
    lastView: null,
    setCurrentView: function(hashTag) {
        hashTag = (hashTag || '').toLowerCase();

        //if (hashTag == 'login') {
        //    var newView = Ext.create({
        //        xtype: hashTag,
        //        routeId: hashTag,  // for existingItem search later
        //        hideMode: 'offsets'
        //    });
        //    return;
        //} 
        var me = this,
            refs = me.getReferences(),
            mainCard = refs.mainCardPanel,
            mainLayout = mainCard.getLayout(),
            navigationList = refs.navigationTreeList,
            store = navigationList.getStore(),
            node = store.findNode('routeId', hashTag) ||
                   store.findNode('viewType', hashTag),
            view = (hashTag == 'login' ? hashTag: '') || (node && node.get('viewType')) || 'page404',
            lastView = me.lastView,
            existingItem = mainCard.child('component[routeId=' + hashTag + ']'),
            newView;

        // Kill any previously routed window
        if (lastView && lastView.isWindow) {
            lastView.destroy();
        }

        lastView = mainLayout.getActiveItem();

        if (!existingItem) {
            newView = Ext.create({
                xtype: view,
                routeId: hashTag,  // for existingItem search later
                hideMode: 'offsets'
            });
        }

        if (!newView || !newView.isWindow) {
            // !newView means we have an existing view, but if the newView isWindow
            // we don't add it to the card layout.
            if (existingItem) {
                // We don't have a newView, so activate the existing view.
                if (existingItem !== lastView) {
                    mainLayout.setActiveItem(existingItem);
                }
                newView = existingItem;
            }
            else {
                // newView is set (did not exist already), so add it and make it the
                // activeItem.
                Ext.suspendLayouts();
                mainLayout.setActiveItem(mainCard.add(newView));
                Ext.resumeLayouts(true);
            }
        }

        navigationList.setSelection(node);

        if (newView.isFocusable(true)) {
            newView.focus();
        }

        me.lastView = newView;
    },

    onNavigationTreeSelectionChange: function (tree, node) {
        var to = node && (node.get('routeId') || node.get('viewType'));

        if (to) {
            this.redirectTo(to);
        }
    },

    onToggleNavigationSize: function () {
        var me = this,
            refs = me.getReferences(),
            navigationList = refs.navigationTreeList,
            wrapContainer = refs.mainContainerWrap,
            collapsing = !navigationList.getMicro(),
            new_width = collapsing ? 64 : 250;

        if (Ext.isIE9m || !Ext.os.is.Desktop) {
            Ext.suspendLayouts();

            refs.senchaLogo.setWidth(new_width);

            navigationList.setWidth(new_width);
            navigationList.setMicro(collapsing);

            Ext.resumeLayouts(); // do not flush the layout here...

            // No animation for IE9 or lower...
            wrapContainer.layout.animatePolicy = wrapContainer.layout.animate = null;
            wrapContainer.updateLayout();  // ... since this will flush them
        }
        else {
            if (!collapsing) {
                // If we are leaving micro mode (expanding), we do that first so that the
                // text of the items in the navlist will be revealed by the animation.
                navigationList.setMicro(false);
            }

            // Start this layout first since it does not require a layout
            refs.senchaLogo.animate({dynamic: true, to: {width: new_width}});

            // Directly adjust the width config and then run the main wrap container layout
            // as the root layout (it and its chidren). This will cause the adjusted size to
            // be flushed to the element and animate to that new size.
            navigationList.width = new_width;
            wrapContainer.updateLayout({isRoot: true});
            navigationList.el.addCls('nav-tree-animating');

            // We need to switch to micro mode on the navlist *after* the animation (this
            // allows the "sweep" to leave the item text in place until it is no longer
            // visible.
            if (collapsing) {
                navigationList.on({
                    afterlayoutanimation: function () {
                        navigationList.setMicro(true);
                        navigationList.el.removeCls('nav-tree-animating');
                    },
                    single: true
                });
            }
        }
    },

    onMainViewRender: function () {
        var that = this;
        if (!that.GetCurrentUser()) {
            that.redirectTo("login");
            return;
        }
        that.authentication(function () {
            var user = that.GetCurrentUser();
            that.loadBaseData();
        }, function () {
            that.redirectTo("login");
        });
    },
    onMainViewAfterrender: function(){
        this.SetCurrentUserName();
        //var that = this;
        // 判断界面大小，小于1366的屏幕以小界面显示
        //if (window.innerWidth < 1366) {
        //    var cmp = Ext.getCmp('main-navigation-btn');
        //    //window.setTimeout(function () { cmp.click(); }, 0.5);
        //    window.setTimeout(function () { that.onToggleNavigationSize(); }, 0.5);
        //}
    },

    onRouteChange: function (id) {
        var that = this;
        if (!that.checkModules()) {
            if (that.GetCurrentUser()) {
                that.setCurrentView(that.loadModules(id));
                that.loadBaseData();
            } else {
                that.setCurrentView('login');
            }
        } else {
            that.setCurrentView(id);
        }
    },

    onSearchRouteChange: function () {
        this.setCurrentView('searchresults');
    },

    onChangePwdClick: function () {
        var that = this;
        that.alertFormWindowSimple('修改密码', [
            {
                fieldLabel: '原密码：',
                name: 'oldpwd'
            }, {
                fieldLabel: '新密码：',
                inputType: 'password',
                name: 'newpwd'
            }, {
                fieldLabel: '确认密码：',
                inputType: 'password',
                name: 'confirmpwd'
            }
        ], function (values) {
            if (!values['oldpwd'] || !values['newpwd'] || !values['confirmpwd'])
                return false;
            if (values['newpwd'].length < 6) {
                that.alertErrorMsg('密码长度不能小于6位！');
                return false;
            }
            if (values['newpwd'] != values['confirmpwd']) {
                that.alertErrorMsg('两次输入的密码不一致！');
                return false;
            }

            var data = {
                OldPwd: md5(values['oldpwd']),
                NewPwd: md5(values['newpwd'])
            }
            var flag = true;
            that.postJsonSync('api/Authentication/chargepwd', data, function () {

            }, function () {
                flag = false;
            });
            return flag;
        })
    },
    onLockClick: function(){
        this.redirectTo('lockscreen');
    },

    onExitClick: function () {
        var that = this;
        Ext.Ajax.request({
            url: 'api/Authentication/Logout',
            method: 'POST',
            success: function (response, opts) {
                var refs = that.getReferences(),
                mainCard = refs.mainCardPanel,
                mainLayout = mainCard.getLayout();

                var items = mainLayout.getLayoutItems();
                Ext.each(items, function (i) {
                    if(i)
                        i.destroy();
                });
                that.removeUserPermission();
                that.redirectTo('login');
            },

            failure: function (response, opts) {
                var refs = that.getReferences(),
                mainCard = refs.mainCardPanel,
                mainLayout = mainCard.getLayout();

                var items = mainLayout.getLayoutItems();
                Ext.each(items, function (i) {
                    if (i)
                        i.destroy();
                });

                that.redirectTo('login');
            }
        });
    }

});
