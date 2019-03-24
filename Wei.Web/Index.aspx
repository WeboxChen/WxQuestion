<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Wei.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>问卷管理系统</title>
    <!-- styles -->
    <link href="resources/bootstrap.3.3.7/content/Content/bootstrap.min.css" rel="stylesheet" />
    <!-- GC -->
    <!-- common style -->
    <link type="text/css" rel="stylesheet" href="ext/classic/theme-triton/resources/theme-triton-all.css"/>
    <!-- style 在最后引用 --> 
    <link type="text/css" rel="stylesheet" href="themes/admin-dashboard/classic/resources/Admin-all.css" />
    <link type="text/css" rel="stylesheet" href="resources/WeiStyle.css" />

    <!-- <x-compile> -->
    <script type="text/javascript" src="ext/ext-all.js"></script>
    <script type="text/javascript" src="scripts/md5.min.js"></script>
    <script type="text/javascript" src="scripts/ZeroClipboard.min.js"></script>
    
    <script src="scripts/audio.min.js"></script>
    <script src="scripts/voice-2.0.js"></script>
<%--    <script src="scripts/pcmdata-2.0.0.min.js"></script>
    <script src="scripts/libamr-2.0.1.min.js"></script>--%>

    <!-- 中文包 -->
    <script type="text/javascript" src="ext/classic/theme-triton/theme-triton.js"></script>
    <script type="text/javascript" src="ext/classic/theme-neptune/theme-neptune.js"></script>

    <!-- reference packages -->
    <script type="text/javascript" src="ext/packages/ux/classic/ux.js"></script>
    <script type="text/javascript" src="ext/packages/legacy/classic/legacy.js"></script>
    <script type="text/javascript" src="ext/packages/charts/classic/charts.js"></script>
    <script type="text/javascript" src="ext/packages/soap/soap.js"></script>

    
    <%--<script type="text/javascript" src="ypu_web.js?v=<%=YPuMES.Core.YPuVersion.CurrentVersion %>"></script>--%>
    <%--<script type="text/javascript" src="spt2a.js"></script>--%>
    <!-- 项目启动项 -->
    <script type="text/javascript" src="app.js"></script>
    <!-- </x-compile> -->
    <script type="text/javascript" src="ext/classic/locale/locale-zh_CN.js"></script>
    <script src="ext/classic/locale/Wei_locale-zh_CN.js"></script>


    <script>
      audiojs.events.ready(function() {
        var as = audiojs.createAll();
      });
    </script>
</head>
<body>
</body>
</html>
