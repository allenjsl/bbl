<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="about_us.aspx.cs" Inherits="Web.GroupEnd.about_us" %>

<%@ Register Src="../../UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/wh/WhLineControl.ascx" TagName="WhLineControl"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany(this.tit.ToString(),this.Page) %></title>
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

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
                <uc4:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">
                            <%= tit %>
                        </span>
                    </h3>
                    <div class="moduel_wrap1 moduel_wrap1_profile">
                        <div class="moduel_wrap1 moduel_wrap2">
                            <asp:Label ID="lblCompanyInfo" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <!--itemListEnd-->
            </div>
        </div>
        <!--mainContentEnd-->
        <div class="clearBoth">
        </div>
        <uc3:WhBottomControl ID="WhBottomControl1" runat="server" />
    </div>
    </form>
</body>
</html>
