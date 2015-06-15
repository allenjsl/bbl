<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamShow.aspx.cs" Inherits="Web.CRM.ProfitStatistical.TeamShow" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <table width="700" border="0" align="center" cellpadding="0" cellspacing="1" style="margin:10px;">
      <tr  class="odd" bgcolor="#BDDCF4">
        <th width="118" >线路名称</th>
        <th width="80" height="30" >团号</th>
        <th width="87" >出团时间</th>
        <th width="80" >人数</th>
        <th width="80" >计调</th>
        <th width="80" >收入</th>
        <th width="87" >支出</th>
        <th width="87" >毛利</th>
      </tr>
      <asp:Repeater runat="server" ID="rptList">
      <ItemTemplate>
      <tr class="even">
        <td align="center"><%# Convert.ToString(Eval("TourType")) == "单项服务" ? "单项服务" : Eval("RouteName")%></td>
        <td height="30" align="center"><%#Eval("TourCode")%></td>
        <td align="center"><%#Eval("LDate","{0:yyyy-MM-dd}")%></td>
        <td align="center"><%#Eval("RealityPeopleNumber")%></td>
        <td align="center"><%#Eval("PlanNames")%></td>
        <td align="center">
            <font class="fbred"><%#Eval("IncomeAmount", "{0:c2}")%></font>
        </td>
        <td align="center">
            <font class="fbred"><%#Eval("OutAmount", "{0:c2}")%></font>
        </td>
        <td align="center">
            <font class="fbred"><%#Eval("GrossProfit", "{0:c2}")%></font>
        </td>
      </tr>
      </ItemTemplate>
      </asp:Repeater>
      <tr class="even">
        <td height="30" colspan="8" align="right" class="pageup">
            <cc1:ExporPageInfoSelect ID="ExporPageInfoSelect1" runat="server" LinkType="3" PageStyleType="NewButton"
                CurrencyPageCssClass="RedFnt" />
        </td>
      </tr>
   </table>
</body>
</html>
