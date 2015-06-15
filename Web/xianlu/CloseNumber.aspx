<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CloseNumber.aspx.cs" Inherits="Web.xianlu.CloseNumber" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="/css/sytle.css" />
    <style type="text/css">
        .span
        {
            color: #021F43;
            font-size: 12px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="20%" bgcolor="#BDDCF4">
                    报名时间
                </th>
                <th width="20%" bgcolor="#BDDCF4">
                    订单号
                </th>
                <th width="20%" bgcolor="#BDDCF4">
                    销售员
                </th>
                <th width="20%" bgcolor="#BDDCF4">
                    客源单位
                </th>
                <th width="20%" bgcolor="#BDDCF4">
                    人数
                </th>
            </tr>
            <cc1:CustomRepeater ID="CloseNumberList" runat="server" EmptyText="<tr><td colspan='5' align='center'>暂无数据!</td></tr>">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center" bgcolor="#E3F1FC">
                            <%# Eval("IssueTime","{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("OrderNo")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%#Eval("SalerName")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <%# Eval("BuyCompanyName")%>
                        </td>
                        <td align="center" bgcolor="#E3F1FC">
                            <font class="fbred">
                                <%# Eval("AdultNumber")%>+<%# Eval("ChildNumber")%></font>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <%# Eval("IssueTime","{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                            <%# Eval("OrderNo")%>
                        </td>
                        <td align="center">
                            <%#Eval("SalerName")%>
                        </td>
                        <td align="center">
                            <%# Eval("BuyCompanyName")%>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# Eval("AdultNumber")%>+<%# Eval("ChildNumber")%></font>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
            <%if (length != 0)
              { %>
            <tr class="odd">
                <td height="30" align="center">
                    <span style="color: #021F43; font-size: 12px; font-weight: bold;">总计</span>
                </td>
                <td height="30" colspan="3">
                </td>
                <td align="center">
                    <span class="span">
                        <asp:Literal ID="PeopleCount" runat="server"></asp:Literal>+<asp:Literal ID="ChildCount" runat="server"></asp:Literal>
                    </span>
                </td>
            </tr>
            <tr>
                <td height="30" colspan="5" align="right" class="pageup">
                    <cc2:ExportPageInfo ID="Close_ExportPageInfo1" runat="server" CurrencyPageCssClass="RedFnt"
                        LinkType="4" />
                </td>
            </tr>
            <%} %>
        </table>
    </div>
    </form>
</body>
</html>
