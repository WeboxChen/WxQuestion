Ext.define('Wei.model.setting.QuestionBankTag', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'string', name: 'name', persist: true },
        { type: 'int', name: 'type', persist: true },
        { type: 'string', name: 'description', persist: true }
    ]
});
