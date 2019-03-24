Ext.define('Wei.model.account.UserAttribute', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'string', name: 'name', persist: true },
        { type: 'string', name: 'displayname', persist: true },
        { type: 'string', name: 'tags', persist: true },
        { type: 'int', name: 'displayorder', persist: true },

    ]
});
