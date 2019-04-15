Ext.define('Wei.model.questions.QuestionItem', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'questionid', persist: true },
        { type: 'int', name: 'questionbank_id', persist: false },
        { type: 'string', name: 'code', persist: true },
        { type: 'string', name: 'text', persist: true }

    ]
});
