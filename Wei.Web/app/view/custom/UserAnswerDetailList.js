Ext.define('Wei.view.custom.UserAnswerDetailList', {
    extend: 'Ext.container.Container',
    alias: 'widget.custom_useranswerdetaillist',

    layout: 'fit',

    items: [
        {
            xtype: 'custom_useranswerdetailgrid',
            selModel: {
                selType: 'checkboxmodel',
                mode: 'SINGLE',
                showHeaderCheckbox: true
            },
            split: true,
            bind: {
                store: '{useranswerdetaillist}',
            }
        }
    ],
    listeners: {
        render: 'onUserAnswerDetailListRender'
    }
});