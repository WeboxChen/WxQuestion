/**
 * @class Ext.chooser.IconBrowser
 * @extends Ext.view.View
 *
 * This is a really basic subclass of Ext.view.View. All we're really doing here is providing the template that dataview
 * should use (the tpl property below), and a Store to get the data from. In this case we're loading data from a JSON
 * file over AJAX.
 */
Ext.define('Wei.view.common.IconBrowser', {
    extend: 'Ext.view.View',
    alias: 'widget.common_iconbrowser',

    uses: 'Ext.data.Store',

    singleSelect: true,
    overItemCls: 'x-view-over',
    selectedItemCls: 'x-view-selected',
    itemSelector: 'div.div_orderfile',
    tpl: [
         '<tpl for=".">',
         '<div class="div_orderfile">',
           '<img src="resources/images/filesIcon/icon{extension}_64.png" alt="" class="img-rounded"/>',
           '<p>{filename}{extension}</p>',
         '</div>',
         '</tpl>',
    ]
});