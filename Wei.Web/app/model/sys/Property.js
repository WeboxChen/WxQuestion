Ext.define('Wei.model.sys.Property', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'string', name: 'pcode', persist: false },
        { type: 'string', name: 'desc', persist: false },
        { type: 'int', name: 'selectvaluetype', persist: true }

    ]
});
