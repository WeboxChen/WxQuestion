Ext.define('Wei.view.account.UserGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.account_usergrid',



    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: false },



        //{ xtype: 'gridcolumn', text: '名字', dataIndex: 'firstname', hidden: false, editor: { xtype: 'textfield' } },
        //{ xtype: 'gridcolumn', text: '姓氏', dataIndex: 'lastname', hidden: false, editor: { xtype: 'textfield' } },
        //{ xtype: 'gridcolumn', text: '电话', dataIndex: 'phone', hidden: false, editor: { xtype: 'textfield' } },
        //{ xtype: 'gridcolumn', text: 'QQ', dataIndex: 'qq', hidden: false, editor: { xtype: 'textfield' } },
        //{ xtype: 'gridcolumn', text: '邮箱', dataIndex: 'email', hidden: false, editor: { xtype: 'textfield' } },

        //{ xtype: 'numbercolumn', text: '状态', dataIndex: 'status', format: '0', hidden: false },



        //{ xtype: 'numbercolumn', text: '关注', dataIndex: 'subscribe', format: '0', hidden: false },
        { xtype: 'gridcolumn', text: '姓氏', dataIndex: 'firstname', hidden: false },
        { xtype: 'gridcolumn', text: '名称', dataIndex: 'lastname', hidden: false },
        { xtype: 'gridcolumn', text: '昵称', dataIndex: 'nickname', hidden: false },
        {
            xtype: 'numbercolumn', text: '性别',
            dataIndex: 'sex', format: '0', hidden: false,
            editor: {
                xtype: 'combobox',
                bind: {
                    store: '{sexstore}'
                },
                displayField: 'name',
                valueField: 'value'
            },
            renderer: function (d) {
                if (d == 2)
                    return "女";
                return "男";
            }
        },

        { xtype: 'datecolumn', text: '出生日期', dataIndex: 'birthdate', hidden: false, editor: { xtype: 'datefield' } },

        { xtype: 'gridcolumn', text: '城市', dataIndex: 'city', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '省', dataIndex: 'province', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '国家', dataIndex: 'country', hidden: false, editor: { xtype: 'textfield' } },
        {
            xtype: 'widgetcolumn', text: '头像', dataIndex: 'headimgurl', hidden: false,
            widget: {
                xtype: 'image',
                bind: {
                    src: '{record.headimgurl}',
                },
                width: 33,
                height: 33
            }
        },

        { xtype: 'gridcolumn', text: '详细地址', dataIndex: 'address', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '身份证号', dataIndex: 'identitycard', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '学历', dataIndex: 'education', hidden: false, editor: { xtype: 'textfield' } },
        {
            xtype: 'booleancolumn', text: '婚姻状况', dataIndex: 'married', hidden: false,
            editor: {
                xtype: 'combobox',
                bind: {
                    store: '{marriedstore}'
                },
                displayField: 'name',
                valueField: 'value'
            },
            trueText: '已婚', falseText: '未婚'
        },

        { xtype: 'datecolumn', text: '最后答题时间', dataIndex: 'lastanswertime', format: 'Y-m-d', hidden: false },

        { xtype: 'gridcolumn', text: '备注', dataIndex: 'remark', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'numbercolumn', text: '分组', dataIndex: 'groupid', format: '0', hidden: false, editor: { xtype: 'numberfield' } },
        { xtype: 'gridcolumn', text: '标签', dataIndex: 'tagids', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'gridcolumn', text: '注册来源', dataIndex: 'channel', hidden: false, editor: { xtype: 'textfield' } },
        { xtype: 'datecolumn', text: '注册日期', dataIndex: 'createtime', format: 'Y-m-d', hidden: false },
        //{ xtype: 'datecolumn', text: '最后登录时间', dataIndex: 'lastlogintime', format: 'Y-m-d H:i', hidden: false },
        //{ xtype: 'gridcolumn', text: '最后登录ip', dataIndex: 'lastloginip', hidden: false },




    ]
});