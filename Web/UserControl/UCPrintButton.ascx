<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCPrintButton.ascx.cs"
    Inherits="Web.UserControl.UCPrintButton" %>
<a href="javascript:void(0);" id="A_Print" onclick="Print(this)">打 印</a>
<script type="text/javascript">
    $(function() {
        $("#A_Print").attr("ContentId", "<%= ContentId %>");
    });
    function Print(obj) {
        if ($("#<%= ContentId %>").find("[id='EmptyData']").length > 0) 
        {
            alert("暂无数据，无法执行打印！");
            return false;
        }
        $(obj).attr("parentUrl", location.href);
        //window.open("/Common/StatisticsPrint.aspx");//Window.Open()方式窗体打开打印
        parent.Boxy.iframeDialog({
        iframeUrl: '/Common/StatisticsPrint.aspx',  
            title: "打印",
            modal: true,
            width: "907px",
            height: "507px",
            data: {
                parentFrame:queryString("iframeId")
            }
        });
        return false;
    }
    function queryString(val) {
        var uri = window.location.search;
        var re = new RegExp("" + val + "\=([^\&\?]*)", "ig");
        return ((uri.match(re)) ? (uri.match(re)[0].substr(val.length + 1)) : null);
    }
</script>

