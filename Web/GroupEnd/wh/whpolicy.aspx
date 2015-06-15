<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="whpolicy.aspx.cs" Inherits="Web.GroupEnd.whpolicy" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc2" %>
<%@ Register Src="../../UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl" TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/wh/WhLineControl.ascx" TagName="WhLineControl" TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=EyouSoft.Common.Utils.GetTitleByCompany("机票政策", this.Page)%></title>
    <link href="../../css/common.css" rel="stylesheet" type="text/css" />
    <link href="../../css/style.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="page">
        <uc1:WhHeadControl ID="WhHeadControl1" runat="server" />
        <!--mainContentStart-->
        <div class="content">
            <div class="content_left">
                <!--loginFromStart-->
                <uc2:WhLoginControl ID="WhLoginControl1" runat="server" />
                <!--loginFromEnd-->
                <uc5:TravelTool ID="TravelTool1" runat="server" />
                <!--infoListStart-->
                <uc3:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">机票政策</span></h3>
                    <div class="moduel_wrap1 moduel_wrap3">
                        <table bordercolor="#999999" border="0" class="item_list">
                            <tbody>
                                <tr class="title">
                                    <td width="40">
                                        序号
                                    </td>
                                    <td width="654">
                                        标题
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptList" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td height="30" align="center">
                                                <%#(pageIndex - 1) * pageSize + Container.ItemIndex + 1%>
                                            </td>
                                            <td align="center">
                                                <a href="whpolicydetailes.aspx?id=<%# Eval("Id")%>"">
                                                    <%# EyouSoft.Common.Utils.GetText( Eval("Title").ToString(),42,true)%></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div class="digg">
                            <cc2:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton" />
                            <asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <!--itemListEnd-->
        </div>
        <!--mainContentEnd-->
        <div class="clearBoth">
        </div>
        <!--linksStart-->
        <uc4:WhBottomControl ID="WhBottomControl1" runat="server" />
    </div>
    </form>
</body>
</html>
