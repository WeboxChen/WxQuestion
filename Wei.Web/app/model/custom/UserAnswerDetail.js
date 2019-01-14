Ext.define('Wei.model.custom.UserAnswerDetail', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'useranswer_id', persist: false },
        { type: 'number', name: 'questionno', persist: false },
        { type: 'date', name: 'start', persist: false },
        { type: 'date', name: 'end', persist: false },
        { type: 'number', name: 'next', persist: false },
        { type: 'string', name: 'answer', persist: false },
        { type: 'string', name: 'questiontext', persist: false },
        { type: 'string', name: 'questionimage', persist: false },
        { type: 'string', name: 'qtype', persist: false },
        { type: 'string', name: 'atype', persist: false },
        { type: 'string', name: 'qtypename', persist: false },
        { type: 'string', name: 'atypename', persist: false }

    ]
});
