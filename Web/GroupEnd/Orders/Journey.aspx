<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Journey.aspx.cs" Inherits="Web.GroupEnd.Orders.Journey" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>查看行程安排</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div style="border-top: 2px #000000 solid;" id="OrderList">
        <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <tr>
                <td>
                    <strong>行程安排：</strong> &nbsp;&nbsp;<asp:Label ID="lblXC" runat="server" Text="">
                    </asp:Label>
                </td>
            </tr>
        </table>
        <table width="100%" border="0" cellpadding="0" cellspacing="1">
            <asp:Repeater ID="rptXC" runat="server">
                <ItemTemplate>
                    <tr bgcolor="<%#(Container.ItemIndex+1)%2==0?"#e3f1fc":"#BDDCF4" %>">
                        <td align="left" height="25" style="padding-left: 10px;" width="23%">
                            <strong>第
                                <%#Container.ItemIndex+1%>天</strong>&nbsp;&nbsp;&nbsp;
                            <%#((DateTime)xcTime).AddDays((int)Container.ItemIndex).ToString("yyyy-MM-dd")%>
                            <%# EyouSoft.Common.Utils.ConvertWeekDayToChinese(((DateTime)xcTime).AddDays((int)Container.ItemIndex))%></strong>
                        </td>
                        <td align="left" style="padding-left: 10px;" width="16%">
                            区间：<strong>
                                <%#Eval("qujian")%></strong>至<strong>
                                    <%#Eval("jiaotong")%></strong>
                        </td>
                        <td align="left" style="padding-left: 10px;" width="19%">
                            住宿：<strong><%#Eval("zhushu")%></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="5" height="25" style="padding-left: 10px;">
                            内容： &nbsp;&nbsp;&nbsp;&nbsp;<%#Eval("content")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Label ID="lblMsg" runat="server" Text="暂无数据！" Visible="false"></asp:Label>
        </table>
    </div>
</body>
</html>
