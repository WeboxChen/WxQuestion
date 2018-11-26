Ext.define('Wei.proxy.WeiTreeApi', {
    extend: 'Ext.data.proxy.Ajax',
    alias: 'proxy.weitreeapi',

    actionMethods: {
        create: 'POST',
        read: 'POST',
        update: 'POST',
        destroy: 'POST'
    },
    //reader: {
    //    type: 'json',
    //    rootProperty: 'data'
    //},

    // 处理序列化数据
    writer: {
        type: 'json',
        writeAllFields: true,//是否写会全部字段
        root: 'data',
        dateFormat: 'Y-m-d H:i:s'
    },

    listeners: {
        exception: function (proxy, response, operation, eOpts) {
            if (response.status == 401)
                if (location.hash && location.hash != '#login')
                    location.hash = 'login'
        }
    }
});