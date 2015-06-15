<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="whpolicydetailes.aspx.cs"
    Inherits="Web.GroupEnd.whpolicydetailes" %>

<%@ Register Src="../../UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/wh/WhLineControl.ascx" TagName="WhLineControl"
    TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany("机票政策", this.Page)%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />    
    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <div id="page">
        <uc1:WhHeadControl ID="WhHeadControl1" runat="server" />
        <!--mainContentStart-->
        <div class="content">
            <div class="content_left">
                <!--loginFromStart-->
                <!--loginFromStart-->
                <uc2:WhLoginControl ID="WhLoginControl1" runat="server" />
                <!--loginFromEnd-->
                <uc5:traveltool id="TravelTool1" runat="server" />
                <!--infoListStart-->
                <uc3:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">机票政策</span></h3>
                    <div class="moduel_wrap1 moduel_wrap1_profile">
                        <table >
                            <tr >
                                <th align="center" >
                                    <asp:Label ID="lblTitle" runat="server" Style="width: 630px; float: left; word-wrap: break-word;"
                                        Font-Size="Large"></asp:Label>
                                </th>
                            </tr>
                            <tr>
                                <td align="center" style="width:630px">
                                    <asp:HyperLink ID="linkFilePath" runat="server" Visible="false" Target="_blank">下载附件</asp:HyperLink>
                                    <hr style="width: 630px;" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-indent: 25px;">
                                    <asp:Label ID="lblContent" runat="server" Style="width: 630px; float: left; word-wrap: break-word;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <!--itemListEnd-->
            </div>
        </div>
        <!--mainContentEnd-->
        <div class="clearBoth">
        </div>
        <!--linksStart-->
        <uc4:WhBottomControl ID="WhBottomControl1" runat="server" />
    </div>
</body>
</html>
