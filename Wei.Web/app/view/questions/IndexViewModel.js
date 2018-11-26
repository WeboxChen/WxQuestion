Ext.define('Wei.view.questions.IndexViewModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.questions',


    stores: {
        questionbanklist: {
            type: 'questions_questionbank',
            autoLoad: true
        }
    }
});
