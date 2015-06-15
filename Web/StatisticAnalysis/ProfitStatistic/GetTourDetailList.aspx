<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetTourDetailList.aspx.cs"
    Inherits="Web.StatisticAnalysis.ProfitStatistic.GetTourDetailList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>

<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style=" vertical-align:middle" align="center">
        <table width="740" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th style="width:40px; text-align:center">序号</th>
                <th width="118" bgcolor="#BDDCF4">
                    线路名称
                </th>
                <th width="80" height="30" bgcolor="#BDDCF4">
                    团号
                </th>
                <th width="87" bgcolor="#BDDCF4">
                    出团时间
                </th>
                <th width="80" bgcolor="#BDDCF4">
                    人数
                </th>
                <th width="80" bgcolor="#BDDCF4">
                    计调
                </th>
                <th width="80" bgcolor="#BDDCF4">
                    收入
                </th>
                <th width="87" bgcolor="#BDDCF4">
                    支出
                </th>
                <th width="87" bgcolor="#BDDCF4">
                    毛利
                </th>
            </tr>
            <cc1:CustomRepeater ID="crp_GetTourDetailList" runat="server">
                <ItemTemplate>
                    <tr class="even">
                        <td style="text-align:center">
                            <%#(PageSize*(PageIndex-1))+Container.ItemIndex+1 %>
                        </td>
                        <td align="center">
                           <%#Eval("RouteName")%>
                        </td>
                        <td height="30" align="center">
                           <%#Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%#Eval("LDate","{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                           <%#Eval("RealityPeopleNumber")%>
                        </td>
                        <td align="center">
                           <%#Eval("PlanNames")%>
                        </td>
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
                <AlternatingItemTemplate>
                  <tr class="odd">
                        <td style="text-align:center">
                            <%#(PageSize*(PageIndex-1))+Container.ItemIndex+1 %>
                        </td>
                        <td align="center">
                           <%#Eval("RouteName")%>
                        </td>
                        <td height="30" align="center">
                           <%#Eval("TourCode")%>
                        </td>
                        <td align="center">
                            <%#Eval("LDate","{0:yyyy-MM-dd}")%>
                        </td>
                        <td align="center">
                           <%#Eval("RealityPeopleNumber")%>
                        </td>
                        <td align="center">
                           <%#Eval("PlanNames")%>
                        </td>
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
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
            <tr id="tbl_ExportPage" runat="server">           
                <td height="30" colspan="8" align="right" class="pageup">
                    <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server"  LinkType="4"/>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
