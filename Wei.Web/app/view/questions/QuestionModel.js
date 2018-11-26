Ext.define('Wei.view.questions.QuestionModel', {
    extend: 'Ext.app.ViewModel',
    alias: 'viewmodel.questions_question',


    stores: {
        questionlist: {
            type: 'questions_questionbank'
        },

    }
});
