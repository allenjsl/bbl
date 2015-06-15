<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoneyShow.aspx.cs" Inherits="Web.CRM.TimeStatistics.MoneyShow" Title="帐龄分析表_销售分析_客户关系管理" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
      <tr  class="odd" bgcolor="#BDDCF4">
        <th width="118" >线路名称</th>
        <th width="80" height="30" >团号</th>
        <th width="87" >出团时间</th>
        <th width="80" >人数</th>
        <th width="80" >客户单位</th>
        <th width="80" >责任销售</th>
        <th width="80" >总收入</th>
        <th width="87" >已收</th>
        <th width="87" >未收</th>
      </tr>
      <asp:Repeater runat="server" ID="rptList">
      <ItemTemplate>
      <tr class="even">
        <td align="center"><%# Eval("RouteName") %></td>
        <td height="30" align="center"><%#Eval("TourNo")%></td>
        <td align="center"><%#Eval("LeaveDate","{0:yyyy-MM-dd}") %></td>
        <td align="center"><%#Eval("PeopleNumber")%></td>
        <td align="center"><%#Eval("BuyCompanyName")%></td>
        <td align="center"><%#Eval("SalerName")%></td>
        <td align="center"><font class="fbred">￥<%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font></td>
        <td align="center"><font class="fbred">
            <%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("HasCheckMoney"))+ Convert.ToDecimal(Eval("NotCheckMoney")))%>
        </font></td>
        <td align="center"><font class="fbred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%></font></td>
      </tr>
      </ItemTemplate>
      </asp:Repeater>
      <tr class="even">
        <td height="30" colspan="9" align="right" class="pageup">
            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                CurrencyPageCssClass="RedFnt" />
        </td>
      </tr>
   </table>
</body>
</html>
