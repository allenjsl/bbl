<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Web.Webmaster._default" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>巴比来项目管理系统WEBMASTER</title>
    <script type="text/javascript" src="/js/jquery.1.5.2.min.js"></script>
    <script type="text/javascript" src="/js/utilsUri.js"></script>
    <link href="/webmaster/images/mdefaultcore.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var m = {
            setFolder: function() {
                $("#mfolders li").not($("#mfolders li.in")).bind("mouseout", function() { this.className = ""; }).bind("mouseover", function() { this.className = "over"; }).bind("click", function() { m.folderClick(this); });
            },
            folderClick: function(folder) {
                $(folder).siblings().not($("#mfolders li.in")).removeClass("on").addClass("");
                $(folder).parent().children().not($("#mfolders li.in")).unbind("mouseout").unbind("mouseover").unbind("click");
                $(folder).siblings().not($("#mfolders li.in")).bind("mouseout", function() { this.className = "" }).bind("mouseover", function() { this.className = "over"; }).bind("click", function() { m.folderClick(this); });
                $(folder).removeClass("over").addClass("on");
            },
            iframeResize: function() {
                var _opiframe = null; var indexwin = null;
                try {
                    if (document.getElementById) {
                        _opiframe = document.getElementById("opiframe"); indexwin = window;
                        if (_opiframe) {
                            if (_opiframe.contentDocument) {
                                _opiframe.height = _opiframe.contentDocument.body.scrollHeight + 10;
                            }
                            else if (_opiframe.document && _opiframe.document.body.scrollHeight) {
                                iframeheight = opiframe.document.body.scrollHeight + 10;
                                windowheight = indexwin.document.body.scrollHeight - 2008;
                                _opiframe.height = (iframeheight < windowheight) ? windowheight : iframeheight;
                            }
                            if (_opiframe.height < document.body.clientHeight - 90) _opiframe.height = document.body.clientHeight - 90;
                        }
                    }
                } catch (e) { }
            },
            gt: function(url) {
                document.getElementById("opiframe").src = utilsUri.createUri(url, { t: new Date().getTime() });
            }
        };

		$(document).ready(function() {
			m.setFolder(); 
			m.iframeResize();
		});
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="mpage">
        <div id="mhead">
            <div id="mheadlinks">
                <div class="left">
                    &nbsp;Welcome！巴比来项目管理系统。
                </div>
                <div class="right">
                    <a href="javascript:void(0)">首页</a>&nbsp;|&nbsp;
                    <a href="javascript:void(0)">设置</a>&nbsp;|&nbsp;
                    <a href="javascript:void(0)">帮助</a>&nbsp;|&nbsp;
                    <a href="logout.aspx" target="opiframe">退出</a>
                </div>
            </div>
            <div id="mheadothers" class="mclearboth">
                &nbsp;
            </div>
        </div>
        <div class="space">
            &nbsp;</div>
        <div id="mbody">
            <div class="left">
                <div class="top">
                    <span>巴比来项目管理</span></div>
                <ul id="mfolders">                    
                    <li><a href="javascript:m.gt('systems.aspx')">子系统管理</a></li>
                    <li><a href="javascript:m.gt('systemadd.aspx')">添加子系统</a></li>
                    <%--<li><a href="javascript:m.gt('cleanuptourdata.aspx')">子系统业务数据清理</a></li>--%>
                    <li class="in">&nbsp;</li>
                    <li><a href="javascript:m.gt('permissions.aspx')">基础权限管理</a></li>
                    <li class="in">&nbsp;</li>
                    <li><a href="javascript:m.gt('smscashier.aspx')">短信充值审核</a></li>
                    <li class="in">&nbsp;</li>
                    <li><a href="javascript:m.gt('self.aspx')">我的信息管理</a></li>
                    <li class="in">&nbsp;</li>                    
                    <li><a href="javascript:m.gt('logout.aspx')">退出管理</a></li>
                </ul>
            </div>
            <div class="right">
                <iframe src="welcome.htm" style="width: 100%;" frameborder="0" id="opiframe" scrolling="no"
                    name="opiframe" onload="m.iframeResize()"></iframe>
            </div>
            <div class="mclearboth">
                &nbsp;</div>
        </div>
    </div>
    </form>
</body>
</html>
