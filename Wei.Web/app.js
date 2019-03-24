Ext.application({
    name: 'Wei',

    // automatically create an instance of Wei.view.Viewport
    // autoCreateViewport: true,
    extend: 'Wei.Application',

    requires: [
        /*********************************************************************************************
         * base 
         */
        'Wei.data.Simulated',
        'Wei.proxy.API',
        'Wei.proxy.WeiApi',
        'Wei.proxy.WeiTreeApi',
        'Wei.model.Base',
        'Wei.model.DataXY',
        'Wei.model.TreeBase',
        'Wei.store.BaseStore',
        'Wei.store.BaseTreeStore',
        'Wei.store.NavigationTree',
        'Wei.view.BaseController',
        'Wei.view.BaseGrid',
        'Wei.view.NonPageBaseGrid',

        /*********************************************************************************************
         * data
         */
        'Wei.data.base.BaseConfig',

        /*********************************************************************************************
         * models
         */
        // setting
        'Wei.model.setting.ModuleType',
        'Wei.model.setting.QuestionBankTag',
        
        // questions
        'Wei.model.questions.Question',
        'Wei.model.questions.QuestionBank',
        'Wei.model.questions.QuestionAnswer',

        // account
        'Wei.model.account.User',
        'Wei.model.account.UserAttribute', 

        // custom
        'Wei.model.custom.UserAnswer',
        'Wei.model.custom.UserAnswerDetail',

        /*********************************************************************************************
         * stores 
         */
        // setting
        'Wei.store.setting.ModuleType',
        'Wei.store.setting.QuestionBankTag',
        
        // questions
        'Wei.store.questions.Question',
        'Wei.store.questions.QuestionBank',
        'Wei.store.questions.QuestionAnswer',

        // account
        'Wei.store.account.User',
        'Wei.store.account.UserAttribute',

        // custom
        'Wei.store.custom.UserAnswer',
        'Wei.store.custom.UserAnswerDetail',

        /*********************************************************************************************
         * base grid 
         */
        
        /*********************************************************************************************
         * views 
         */
        // authentication
        'Wei.view.authentication.AuthenticationModel',
        'Wei.view.authentication.AuthenticationController',
        'Wei.view.authentication.LockingWindow',
        'Wei.view.authentication.Dialog',
        'Wei.view.authentication.LockScreen',
        'Wei.view.authentication.Login',
        'Wei.view.authentication.PasswordReset',
        'Wei.view.authentication.Register',

        // main
        'Wei.view.main.MainModel',
        'Wei.view.main.MainController',
        'Wei.view.main.MainContainerWrap',
        'Wei.view.main.Main',

        // pages
        'Wei.view.pages.ErrorBase',
        'Wei.view.pages.BlankPage',
        'Wei.view.pages.Error404Window',
        'Wei.view.pages.Error500Window',
       
        // common
        'Wei.view.common.Window',
        'Wei.view.common.IconBrowser',
        'Wei.view.common.FileInfo',

        // setting
        'Wei.view.setting.ModuletypeController',
        'Wei.view.setting.ModuleTypeGrid',
        'Wei.view.setting.ModuleTypeInfo',
        'Wei.view.setting.QuestionBankTagController',
        'Wei.view.setting.QuestionBankTagGrid',
        'Wei.view.setting.QuestionBankTagInfo',

        // questions
        'Wei.view.questions.IndexViewModel',
        'Wei.view.questions.DetailViewModel', 

        'Wei.view.questions.IndexController',
        'Wei.view.questions.DetailController',

        'Wei.view.questions.QuestionBankGrid',
        'Wei.view.questions.QuestionBankInfo',
        'Wei.view.questions.QuestionGrid',
        'Wei.view.questions.QuestionAnswerGrid',
        'Wei.view.questions.QuestionInfo',

        'Wei.view.questions.QuestionList',
        'Wei.view.questions.QuestionBankList',
        'Wei.view.questions.QuestionBankDetail',
        'Wei.view.questions.Index',

        // answers
        'Wei.view.custom.IndexViewModel',

        'Wei.view.custom.IndexController',

        'Wei.view.custom.UserAnswerGrid',
        'Wei.view.custom.UserAnswerDetailGrid', 
        'Wei.view.custom.UserAnswerList',
        'Wei.view.custom.UserAnswerDetailList',
        'Wei.view.custom.CustomDetail',

        'Wei.view.custom.Index', 

        // userinfo
        'Wei.view.account.UserViewModel',
        'Wei.view.account.UserController',
        'Wei.view.account.UserGrid',

        // user attribute
        'Wei.view.account.UserAttributeViewModel',
        'Wei.view.account.UserAttributeController',
        'Wei.view.account.UserAttributeGrid',

        //'Wei.Application'
    ]
});
