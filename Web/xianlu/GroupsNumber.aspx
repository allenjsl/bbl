<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupsNumber.aspx.cs" Inherits="Web.xianlu.GroupsNumber" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="Stylesheet" type="text/css" href="/css/sytle.css" />
    <style type="text/css">
        .text
        {
            width: 50px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="6%" height="30" bgcolor="#BDDCF4">
                    团号
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    出团时间
                </th>
                <th width="33%" bgcolor="#BDDCF4">
                    团队名称
                </th>
                <th width="6%" bgcolor="#BDDCF4">
                    天数
                </th>
                <th width="10%" bgcolor="#BDDCF4">
                    实收人数
                </th>
                <th width="12%" bgcolor="#BDDCF4">
                    收入
                </th>
                <th width="8%" bgcolor="#BDDCF4">
                    支出
                </th>
                <th width="10%" bgcolor="#BDDCF4">
                    毛利
                </th>
            </tr>
            <cc1:CustomRepeater ID="GroupsNumberList" runat="server" EmptyText="<tr><td colspan='8' align='center'>暂无数据!</td></tr>">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <%# Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%# Eval("LDate", "{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                            <%# Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%# Eval("TourDays")%>
                        </td>
                        <td align="center">
                            <%# Eval("PeopleNumberShiShou")%>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalIncome").ToString()))%></font>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalExpenses").ToString()))%></font>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroString( (EyouSoft.Common.Utils.GetDecimal(Eval("TotalIncome").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("TotalExpenses").ToString())).ToString("0.00"))%></font>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <%# Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%# Eval("LDate", "{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                            <%# Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%# Eval("TourDays")%>
                        </td>
                        <td align="center">
                            <%# Eval("PeopleNumberShiShou")%>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalIncome").ToString()))%></font>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(EyouSoft.Common.Utils.GetDecimal(Eval("TotalExpenses").ToString()))%></font>
                        </td>
                        <td align="center">
                            <font class="fbred">
                                <%# EyouSoft.Common.Utils.FilterEndOfTheZeroString( (EyouSoft.Common.Utils.GetDecimal(Eval("TotalIncome").ToString()) - EyouSoft.Common.Utils.GetDecimal(Eval("TotalExpenses").ToString())).ToString("0.00"))%></font>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
            <%if (length != 0)
              { %>
            <tr class="odd">
                <td height="30" align="center">
                    <span style="color: #021F43; font-size: 12px; font-weight: bold;">总计：</span>
                </td>
                <td align="center" colspan="3">
                </td>
                <td align="center">
                    <span style="color: #021F43; font-size: 12px; font-weight: bold;">
                        <asp:Literal ID="PeopleCount" runat="server"></asp:Literal></span>
                </td>
                <td align="center">
                    <span style="color: #021F43; font-size: 12px; font-weight: bold;">￥<asp:Literal ID="TotalIncomeCount"
                        runat="server"></asp:Literal></span>
                </td>
                <td align="center">
                    <span style="color: #021F43; font-size: 12px; font-weight: bold;">￥<asp:Literal ID="TotalExpensesCount"
                        runat="server"></asp:Literal></span>
                </td>
                <td align="center">
                    <span style="color: #021F43; font-size: 12px; font-weight: bold;">￥<asp:Literal ID="GrossProfitCount"
                        runat="server"></asp:Literal></span>
                </td>
            </tr>
            <tr>
                <td height="30" colspan="8" align="right" class="pageup">
                    <cc2:ExportPageInfo ID="Group_ExportPageInfo1" runat="server" CurrencyPageCssClass="RedFnt"
                        LinkType="4" />
                </td>
            </tr>
            <%} %>
        </table>
    </div>
    </form>
</body>
</html>
