Ext.define('Wei.model.questions.QuestionBank', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'type', persist: true },
        { type: 'string', name: 'title', persist: true },
        { type: 'string', name: 'description', persist: true },
        { type: 'string', name: 'remark', persist: true },
        { type: 'int', name: 'status', persist: true },
        { type: 'date', name: 'createtime', persist: false },
        { type: 'int', name: 'creatorid', persist: false }
    ]
});
