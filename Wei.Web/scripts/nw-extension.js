var nwobj;


nwobj = (function () {
    if (!require || !process) return null;

    var fs = require('fs');
    var proc = require('child_process');

    var fpath = process.cwd() + "\\program\\ReportPrinter\\ReportPrinter.exe";
    // ReportPath: "/sunny/part",
    // Params: { Id: "3075" }
    var aa = {
        ReportServerHost: "http://localhost/ReportServer_mssql2016",
        ReportPath: "",
        RenderType: "PDF",
        Params: {}
    };

    this.openFile = function (path) {
        if (!path)
            path = fpath + " \"1\" \"" + JSON.stringify(aa).replace(/\\/g, "\\\\").replace(/\"/g, "\\\"") + "\"";

        proc.exec(path, function (err, data) {
            if (data) {
                var obj = JSON.parse(data);
                if (obj && obj.msg) {
                    console.log(obj.msg);
                }
            }
        });
    };

    this.printLabel = function (reportPath, params, fn1, fn2) {
        aa.ReportPath = reportPath;
        aa.Params = params;
        // 组成参数路径
        var path = fpath + " \"1\" \"" + JSON.stringify(aa).replace(/\\/g, "\\\\").replace(/\"/g, "\\\"") + "\"";
        proc.exec(path, function (err, data) {
            console.log(data);
            if (err) {
                console.log(err);
                alert(err);
                return;
            }
            if (data) {
                var obj = JSON.parse(data);
                if (obj.success)
                    fn1(obj);
                else
                    fn2(obj);
            }
        })
    };

    return this;
})();