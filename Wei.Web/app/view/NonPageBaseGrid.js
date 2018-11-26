/*************************************************************************************
 * 基础表格
 * checkcolumn 列类型，支持field: Boolean， 
 ** 默认禁用，如需要编辑， 在列上标识 (_isEdit=1)  编辑模式时启用
 * 
 *************************************************************************************/
Ext.define('Wei.view.NonPageBaseGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.nonpagebasegrid',

    layout: 'fit',
    selModel: {
        selType: 'checkboxmodel',
        mode: 'MULTI',
        showHeaderCheckbox: true
    },
    scrollable: true
});