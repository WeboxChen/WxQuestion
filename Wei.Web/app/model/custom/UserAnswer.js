Ext.define('Wei.model.custom.UserAnswer', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'questionbank_id', persist: false },
        { type: 'int', name: 'user_id', persist: false },
        { type: 'string', name: 'nickname', persist: false },
        { type: 'date', name: 'begintime', persist: false },
        { type: 'date', name: 'completedtime', persist: false },
        { type: 'string', name: 'questionbank', persist: false },
        { type: 'int', name: 'questionbanktype', persist: false },
        { type: 'string', name: 'questionbanktypename', persist: false },
        { type: 'string', name: 'status', persist: false }

    ]
});
