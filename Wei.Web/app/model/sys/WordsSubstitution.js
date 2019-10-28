Ext.define('Wei.model.sys.WordsSubstitution', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'questionbankid', persist: true },
        { type: 'string', name: 'questionbankname', persist: false },
        { type: 'string', name: 'words1', persist: true },
        { type: 'string', name: 'words2', persist: true }

    ]
});
