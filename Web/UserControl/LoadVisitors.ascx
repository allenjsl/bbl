<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadVisitors.ascx.cs" Inherits="Web.UserControl.LoadVisitors" %>
<a href="javascript:;" id="linkLoadVisitors"><img src="/images/sanping_03.gif">导入</a>
<script type="text/javascript">
$(function(){
    $("#linkLoadVisitors").click(function(){
        var ciframeId = "<%=currentPageIframeId %>";
        parent.Boxy.iframeDialog({
            iframeUrl: "/common/LoadVisitors.aspx",
            width: "853px",
            height: "514px",
            async: false,
            title: "导入游客信息",
            modal: true,
            data:{
                topID:ciframeId,
                type:"otherload"
            }
        });
        return false;
    });
});
</script>