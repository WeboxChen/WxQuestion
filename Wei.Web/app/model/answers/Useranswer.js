Ext.define('Wei.model.answers.UserAnswer', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
{ type: 'int', name: 'user_id', persist: false },
{ type: 'int', name: 'questionbank_id', persist: false },
{ type: 'date', name: 'begintime', persist: false },
{ type: 'date', name: 'completedtime', persist: false },

    ]
});
