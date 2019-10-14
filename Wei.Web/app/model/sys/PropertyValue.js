Ext.define('Wei.model.sys.PropertyValue', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'propertyid', persist: true },
        { type: 'string', name: 'text', persist: true },
        { type: 'int', name: 'isdef', persist: true },
        { type: 'int', name: 'sort', persist: true }

    ]
});
