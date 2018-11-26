Ext.define('Wei.model.answers.UserAnswerDetail', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
{ type: 'int', name: 'useranswer_id', persist: false },
{ type: 'int', name: 'question_id', persist: false },
{ type: 'string', name: 'answer', persist: false },
{ type: 'int', name: 'next', persist: false },
{ type: 'date', name: 'start', persist: false },
{ type: 'date', name: 'end', persist: false },

    ]
});
