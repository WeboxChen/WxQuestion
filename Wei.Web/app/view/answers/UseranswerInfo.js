Ext.define('Wei.view.answers.UserAnswerInfo', {
    extend: 'Ext.form.Panel',
    alias: 'widget.answers_useranswerinfo',

    layout: 'column',

    items: [
        
        { 
            xtype: 'numberfield', 
            emptyText: 'id', 
            name: 'id', 
            allowDecimals: false 
        },
        
    ]
});