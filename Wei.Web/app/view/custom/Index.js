Ext.define('Wei.view.custom.Index', {
    extend: 'Ext.container.Container',
    alias: 'widget.custom',

    controller: 'custom',
    viewModel: 'custom',

    itemId: 'customContainer',

    layout: 'fit',
    listeners: {
        show: 'onCustomShow'
    }


});