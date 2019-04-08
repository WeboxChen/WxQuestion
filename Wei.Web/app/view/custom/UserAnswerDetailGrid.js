Ext.define('Wei.view.custom.UserAnswerDetailGrid', {
    extend: 'Wei.view.BaseGrid',
    alias: 'widget.custom_useranswerdetailgrid',
    
    //bind: {
    //    store: '{useranswerdetaillist}',
    //},

    tbar: [
        {
            text: '添加',
            handler: 'onAdd'
        }, {
            xtype: 'splitbutton',
            text: '修改',
            handler: 'onEdit',
            menu: {
                items: [
                    {
                        text: '单项修改',
                        handler: 'onEdit'
                    },
                    {
                        text: '批量修改',
                        handler: 'onMultiEdit'
                    }
                ]
            }
        //}, {
        //    text: '删除',
        //    handler: 'onDel'
        }, {
            text: '取消',
            handler: 'onCancel'
        }, {
            text: '保存',
            handler: 'onSave'
        }
    ],

    columns: [
        { xtype: 'numbercolumn', text: 'id', dataIndex: 'id', format: '0', hidden: true },
        { xtype: 'numbercolumn', text: '题号', dataIndex: 'questionno', hidden: false, width: 60 },
        { xtype: 'gridcolumn', text: '题目', dataIndex: 'questiontext', width: 280, hidden: false },
        { xtype: 'gridcolumn', text: '题目图片', dataIndex: 'questionimage', hidden: true },
        { xtype: 'gridcolumn', text: '题目类型', dataIndex: 'qtypename', hidden: false, width: 80 },
        { xtype: 'gridcolumn', text: '答题类型', dataIndex: 'atypename', hidden: false, width: 80 },


        { xtype: 'gridcolumn', text: '客户答案', dataIndex: 'answer', hidden: false, editor: { xtype: 'textfield' } },
        {
            xtype: 'widgetcolumn', text: '答题语音', 
            width: 120,
            widget: {
                xtype: 'buttongroup',
                //bind: {
                //    _wei: '{record.viocepath}'
                //},
                items: [
                    {
                        xtype: 'button',
                        text: 'Play',
                        scale: 'small',
                        //bind: {
                        //    _data: '{viocepath}'
                        //},
                        handler: 'onAnswerViocePlay'
                    },
                    {
                        xtype: 'button',
                        text: 'Stop',
                        scale: 'small',
                        handler: 'onAnswerVioceStop'
                    }
                ]
                //bind: {
                //    html: '<audio src="{record.voicepath}"  controls="controls" ></audio>'
                //}
                //bind: '{record.voicepath}',
                //html: '<audio src="{record.voicepath}" controls="controls" />'
                //bind: {
                    //tpl: '<audio src="123" src="{0}"  controls="controls" ></audio>'
                //}
            }
        },
        { xtype: 'datecolumn', text: '答题开始时间', dataIndex: 'start', format: 'Y-m-d H:i', hidden: false, width: 120 },
        { xtype: 'datecolumn', text: '答题结束时间', dataIndex: 'end', format: 'Y-m-d H:i', hidden: false, width: 120 }
        
    ]
});