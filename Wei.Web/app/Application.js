Ext.define('Wei.Application', {
    extend: 'Ext.app.Application',
    
    stores: [
        'NavigationTree',
    ],
    
    mainView: 'Wei.view.main.Main',
    onAppUpdate: function(){
        Ext.Msg.confirm('Application Update', 'This application has an update, reload?',
            function (choice) {
                if (choice === 'yes') {
                    window.location.reload();
                }
            }
        );
    }
})