Ext.define('Wei.model.setting.ModuleType', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'string', name: 'name', persist: true },
        { type: 'string', name: 'remark', persist: true }
    ]
});
