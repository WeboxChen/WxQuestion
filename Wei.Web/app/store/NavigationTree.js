// 左侧树形菜单
Ext.define('Wei.store.NavigationTree', {
    extend: 'Wei.store.BaseTreeStore',

    storeId: 'NavigationTree',
    alias: 'store.NavigationTree',

    fields: [{
        name: 'text'
    }],

    proxy: {
        type: 'weitreeapi',
        api: {
            read: 'api/common/getmodule'
        }
    },

    root: {
        expanded: true,
        children: [
            {
                text: '系统题库',
                expanded: true,
                children: [
                    {
                        text: '题库',
                        viewType: 'questions',
                        leaf: true
                    },
                    {
                        text: '题库分类',
                        viewType: 'setting_moduletypegrid',
                        leaf: true
                    //},
                    //{
                    //    text: '题库标签',
                    //    viewType: 'setting_questionbanktaggrid',
                    //    leaf: true
                    }
                ]
            },
            {
                text: '答题统计',
                expanded: false,
                children: [
                    {
                        text: '答题记录',
                        leaf: true,
                        viewType: 'custom'
                    }
                ]
            },
            {
                text: '用户管理',
                expanded: false,
                children: [
                    {
                        text: '用户信息',
                        leaf: true,
                        viewType: 'account_userlist'
                    //}, {
                    //    text: '客户属性',
                    //    leaf: true,
                    //    viewType: 'account_userattributegrid'
                    }
                ]
            },
            {
                text: '系统设置',
                expanded: false,
                children: [
                    {
                        text: '基础数据',
                        leaf: true,
                        viewType: 'setting_basic'
                    }, {
                        text: '修正簿',
                        leaf: true,
                        viewType: 'setting_wordssubtitutionlist'
                    }
                ]
            },
            {
                text: '登录',
                iconCls: 'x-fa fa-user',
                viewType: 'login',
                leaf: true
            }
        ]
    },

    listeners: {
        load: function (s, records, successful, operation, node) {
            if (successful) {
                if (records.length == 0) {
                    Ext.Msg.alert('无权限操作！请联系管理员');
                    location.href = '';
                }
            } else {
                Ext.Msg.alert('服务器错误！');
                location.href = '';
            }
        }
    }
});
