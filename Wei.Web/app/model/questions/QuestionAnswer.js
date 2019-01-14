Ext.define('Wei.model.questions.QuestionAnswer', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'questionid', persist: true },
        { type: 'int', name: 'questionbank_id', persist: false },
        { type: 'string', name: 'answerkeys', persist: true },
        { type: 'float', name: 'next', persist: true },
        { type: 'float', name: 'sort', persist: true }
    ]
});
