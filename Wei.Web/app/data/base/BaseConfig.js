Ext.define('Wei.data.base.BaseConfig', {
    extend: 'Wei.data.Simulated',

    data: [
        {
            // http://180.169.5.206:8013/ReportServer_MSSQL2014/    美高
            // http://192.168.10.116/ReportServer/                  客户
            // http://localhost/ReportServer_mssql2016/             本地
            reportServer: 'http://192.168.2.194/ReportServer_mssql2016/',
        }
    ]
});
