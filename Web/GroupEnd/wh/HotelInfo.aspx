<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HotelInfo.aspx.cs" Inherits="Web.GroupEnd.wh.HotelInfo" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc3" %>
<%@ Register Src="/UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl"
    TagPrefix="uc1" %>
<%@ Register Src="/UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc2" %>
<%@ Register Src="/UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl"
    TagPrefix="uc3" %>
<%@ Register Src="/UserControl/wh/WhLineControl.ascx" TagName="WhLineControl"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/wh/TravelTool.ascx" TagName="TravelTool" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<head id="Head1" runat="server">
    <title><%=EyouSoft.Common.Utils.GetTitleByCompany("酒店",this.Page) %></title>
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/tipsy.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body id="Body1" runat="server">
    <form id="Form1" runat="server">
    <div id="page">
        <uc1:WhHeadControl ID="WhHeadControl1" runat="server" />
        <!--mainContentStart-->
        <div class="content">
            <div class="content_left">
                <!--loginFromStart-->
                <uc3:WhLoginControl ID="WhLoginControl1" runat="server" />
                <!--loginFromEnd-->
                <uc5:traveltool id="TravelTool1" runat="server" />
                <!--infoListStart-->
                <uc4:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">酒店详情</span></h3>
                    <div class="moduel_wrap1 moduel_wrap1_profile">
                        <style>
                            .moduel_wrap1_profile table
                            {
                                border-collapse: collapse;
                                width: 100%;
                            }
                            .moduel_wrap1_profile table, .moduel_wrap1_profile table tr, .moduel_wrap1_profile table tr td
                            {
                                border: 1px solid #ccc;
                                padding: 5px;
                            }
                        </style>
                        <table cellspacing="1" cellpadding="1" border="1" width="550">
                            <tbody>
                                <tr>
                                    <td align="center" width="50px">
                                        酒店名称
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblHotelName" runat="server" Text="暂无"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" width="50px">
                                        星级
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblHotelStar" runat="server" Text="暂无"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" width="50px">
                                        地址
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblHotelAddress" runat="server" Text="暂无"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" width="50px">
                                        酒店图片
                                    </td>
                                    <td colspan="2" width="585px">
                                        <%=photos %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" width="50px">
                                        酒店简介
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblHotelInfo" runat="server" Text="暂无"></asp:Label>
                                    </td>
                                </tr>
                                <asp:Repeater runat="server" ID="repRoom">
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center" rowspan="3" width="50px">
                                                房间
                                            </td>
                                            <td align="right" width="50px">
                                                房型
                                            </td>
                                            <td width="368">
                                                <%#Eval("Name").ToString()!=""?Eval("Name"):"暂无"%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" width="50px">
                                                早餐
                                            </td>
                                            <td>
                                                <%#Eval("IsBreakfast").ToString().ToUpper()=="TRUE"?"有":"无" %>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="23" align="right" width="50px">
                                                价格
                                            </td>
                                            <td>
                                                <font style="color: Red">￥<%# Convert.ToDecimal(Eval("SellingPrice")).ToString("f2") %></font>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <div style="width: 100%; text-align: center">
                            <a  href="javascript:history.go(-1);"  style="font-size:14px;color:#004784" >返  回</a>
                        </div>
                </div>
                <!--itemListEnd-->
                <!--picListStart-->
                <!--picListEnd-->
            </div>
        </div>
        <!--mainContentEnd-->
        <div class="clearBoth">
        </div>
        <uc2:WhBottomControl ID="WhBottomControl1" runat="server" />
    </div>
    <div id="trMsg" style="color: Red; display: none;">
        <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px;">
            <tr>
                <td height="60">
                    <span style="font-size: 14px; font-weight: bold; color: #FF0000; text-indent: 24px;">
                        尊敬的客户： 您已经登录成功 若系统后台没有自动弹出，请点这里进入系统后台！ <a id="linkManage" href="#" style="text-decoration: underline"
                            target="_self">进入系统后台</a></span>
                </td>
            </tr>
        </table>
    </div>

    <script src="/js/jquery.tipsy.js" type="text/javascript"></script>

    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>

    </form>
</body>
</html>
