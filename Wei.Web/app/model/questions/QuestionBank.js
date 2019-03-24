Ext.define('Wei.model.questions.QuestionBank', {
    extend: 'Wei.model.Base',

    fields: [
        { type: 'int', name: 'id', persist: true },
        { type: 'int', name: 'type', persist: true },
        { type: 'string', name: 'title', persist: true },
        { type: 'string', name: 'description', persist: true },
        { type: 'string', name: 'remark', persist: true },
        { type: 'bool', name: 'autoresponse', persist: true },
        { type: 'string', name: 'responsekeywords', persist: true },
        { type: 'int', name: 'status', persist: true },
        { type: 'date', name: 'expiredatebegin', persist: true },
        { type: 'date', name: 'expiredateend', persist: true },
        { type: 'date', name: 'createtime', persist: false },
        { type: 'int', name: 'creatorid', persist: false },
        { type: 'string', name: 'userattributes', persist: true }
    ]
});
