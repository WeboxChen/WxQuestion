/*************************************************************************************
 * 基础表格
 * checkcolumn 列类型，支持field: Boolean， 
 ** 默认禁用，如需要编辑， 在列上标识 (_isEdit=1)  编辑模式时启用
 * 
 *************************************************************************************/
Ext.define('Wei.view.BaseGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.basegrid',

    selModel: {
        selType: 'checkboxmodel',
        mode: 'SINGLE',
        showHeaderCheckbox: true,
        checkOnly: true
    },
    layout: 'fit',
    scrollable: true,
    
    features: [
        { ftype: 'grouping' }
    ],

    bbar: {
        xtype: 'pagingtoolbar',
        displayInfo: true
    },
    initComponent: function () {
        var that = this;
        //var rightClick = new Ext.menu.Menu({
        //    items: [
        //        {
        //            text: '复制',
        //            handler: function (t) {
        //                var menu = t.up('menu');
        //                var clip = new ZeroClipboard();
        //                clip.setText(menu._content)
        //                //console.log(menu._content);
        //                //console.log(clip);
        //            }
        //        }
        //    ]
        //});
        //that.on({
        //    cellcontextmenu: {
        //        fn: function (t, td, cellindex, record, tr, rowindex, e, eopts) {
        //            e.stopEvent();
        //            rightClick._content = td;
        //            //if (td.firstElementChild)
        //            //    rightClick._content = td.firstElementChild.innerText;
        //            //else
        //            //    rightClick._content = td.innerText;
        //            rightClick.showAt(e.getXY());
        //        },
        //        scope: that
        //    }
        //})
        that.callParent();
    }
});