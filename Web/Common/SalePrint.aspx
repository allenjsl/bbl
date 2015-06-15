<%@ Page Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true" CodeBehind="SalePrint.aspx.cs" Inherits="Web.Common.SalePrint" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
<style type="text/css" media="screen">
    #printtable
    {
        border-collapse: collapse; /* 关键属性：合并表格内外边框(其实表格边框有2px，外面1px，里面还有1px哦) */
        border: solid #000; /* 设置边框属性；样式(solid=实线)、颜色(#999=灰) */
        border-width: 1px 0 0 1px; /* 设置边框状粗细：上 右 下 左 = 对应：1px 0 0 1px */
    }
    #printtable th, #printtable td
    {
        border: solid #000;
        border-width: 0 1px 1px 0;
        padding: 2px;
    }
</style>
<div id="printDiv"></div>
<script type="text/javascript">
    $(function() {
        if(!$("#printDiv").html())
        {
            var url='<%=url %>';
            $.ajax({
                type: 'get',
                url: url,
                dataType: "html",
                success: function(html) { 
                    var PrintDom = $(html).find("#printtable");
                    $(PrintDom).find("*").each(function() {
                        $(this).removeAttr("href");
                        $(this).removeAttr("bgColor");
                    });
                    $("#printDiv").append(PrintDom);
                }
            });
        }
    });
</script>
</asp:Content>
