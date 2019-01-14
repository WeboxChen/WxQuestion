Ext.define('Wei.view.custom.CustomDetail', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.custom_detail',
    
    tabBarHeaderPosition: 0,
    tools: [
        {
            type: 'close',
            callback: 'onBackBtnClick'
        }
    ],

    layout: 'fit',
    items: [
        {
            title: '答题明细',
            xtype: 'custom_useranswerdetaillist'
        }
    ],

    listeners: {
        beforerender: 'onDetailBeforeRender'
    }
});
