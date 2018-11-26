Ext.define('Wei.view.common.FileInfo', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.common_fileinfo',

    // config field for data bind
    config: {
        _sid: 0
    },

    listeners: {
        afterrender: function (t) {
            // save default values for form
            //var formcmp = t.down('form'),
            //    form = formcmp.getForm();
            //form.setValues({ filetype: t.ftype, SId: t._sid });

            // load iconbrowser content
            var iconbrowsercmp = t.down('common_iconbrowser'),
                mainview = t.up('[viewModel]'),
                viewmodel = mainview.getViewModel(),
                store = viewmodel.getStore(t._store);
            
            iconbrowsercmp.setStore(store);
        }
    },

    border: 1,
    headerPosition: 'left',
    margin: '2 0 2 0',

    tbar: [
        {
            xtype: 'form',
            items: [
                {
                    xtype: 'hidden',
                    name: 'filetype'
                }, {
                    xtype: 'hidden',
                    name: 'SId'
                }, {
                    xtype: 'fileuploadfield',
                    ypu_type: 'btn_upload',
                    ypu_btn_type: 'btn_upload',
                    buttonOnly: true,
                    hideLabel: true,
                    width: 50,
                    margin: '3 0 0',
                    buttonText: '上传',
                    name: 'btn_file',
                    listeners: {
                        change: "onFileUpload"
                    }
                }
            ]
        }, {
            text: '下载',
            ypu_type: 'btn_download',
            ypu_btn_type: 'btn_download',
            handler: 'onFileDownClick'
        }, {
            text: '删除',
            handler: 'onFileDelClick'
        }
    ],

    items: [
        {
            xtype: 'common_iconbrowser',
            scrollable: true,
            listeners: {
                itemdblclick: 'onFileDownClick'
            }
        }
    ]

})