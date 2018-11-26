Ext.define('Wei.view.main.Main', {
    extend: 'Ext.container.Viewport',

    controller: 'main',
    viewModel: 'main',

    cls: 'sencha-dash-viewport',
    itemId: 'mainView',
    id: 'mainView',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    listeners: {
        render: 'onMainViewRender',
        afterrender: 'onMainViewAfterrender',
        show: 'onMainViewAfterrender'
    },

    items: [
        {
            xtype: 'toolbar',
            cls: 'sencha-dash-dash-headerbar shadow',
            height: 64,
            itemId: 'headerBar',
            items: [
                {
                    xtype: 'component',
                    reference: 'senchaLogo',
                    cls: 'sencha-logo',
                    html: '<div class="main-logo"><img src="resources/images/company-logo.png" width="28" height="28">逸璞</div>',
                    width: 250
                },
                {
                    margin: '0 0 0 8',
                    ui: 'header',
                    iconCls:'x-fa fa-navicon',
                    id: 'main-navigation-btn',
                    handler: 'onToggleNavigationSize'
                },
                '->',
                //{
                //    iconCls:'x-fa fa-search',
                //    ui: 'header',
                //    href: '#searchresults',
                //    hrefTarget: '_self',
                //    tooltip: 'See latest search'
                //},
                {
                    xtype: 'splitbutton',
                    cls: 'top-user-name',
                    text: 'Administrator',
                    id: 'head_username',
                    menu: {
                        id: 'view_user_menu',
                        items: [
                            {
                                text: '锁定',
                                handler: 'onLockClick'
                            },
                            {
                                text: '修改密码',
                                handler: 'onChangePwdClick'
                            },
                            {
                                text: '退出',
                                handler: 'onExitClick'
                            }
                        ]
                    }
                }
            ]
        },
        {
            xtype: 'maincontainerwrap',
            id: 'main-view-detail-wrap',
            reference: 'mainContainerWrap',
            flex: 1,
            items: [
                {
                    xtype: 'treelist',
                    reference: 'navigationTreeList',
                    itemId: 'navigationTreeList',
                    ui: 'navigation',
                    store: 'NavigationTree',
                    style: 'overflow-y: visible',
                    //bind: {
                    //    store: '{navigationTree}',
                    //},
                    //scrollable: true,
                    width: 250,
                    expanderFirst: false,
                    expanderOnly: false,
                    singleExpand: true,
                    listeners: {
                        selectionchange: 'onNavigationTreeSelectionChange'
                    },
                    ////micro: window.innerWidth >= 1366
                    //initComponent: function () {
                    //    var config = this.getConfig();
                    //    console.log(config)
                    //    if (window.innerWidth >= 1366) {
                            
                    //        config['micro'] = true;
                    //        this.initConfig(config);
                    //    }
                    //}
                },
                {
                    xtype: 'container',
                    flex: 1,
                    reference: 'mainCardPanel',
                    cls: 'sencha-dash-right-main-container',
                    itemId: 'contentPanel',
                    layout: {
                        type: 'card',
                        anchor: '100%'
                    }
                }
            ]
        }
    ]
});
