Ext.define('Wei.model.questions.Question', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'questionbank_id', persist: true },
        { type: 'string', name: 'atype', persist: true },
        { type: 'string', name: 'qtype', persist: true },
        { type: 'string', name: 'text', persist: true },
        { type: 'string', name: 'imagecode', persist: true },
        { type: 'string', name: 'answer', persist: true },
        { type: 'float', name: 'next1', persist: true },
        { type: 'float', name: 'next2', persist: true },
        { type: 'float', name: 'sort', persist: true },
        { type: 'string', name: 'groupno', persist: true },
        { type: 'string', name: 'mtype', persist: true }
    ]
});
