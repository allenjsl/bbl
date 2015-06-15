<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetArrearageList.aspx.cs"
    Inherits="Web.StatisticAnalysis.Ageanalysis.GetArrearageList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <style>
        body
        {
            padding: 0;
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="715px" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="120px" bgcolor="#BDDCF4">
                    线路名称
                </th>
                <th width="80px" height="30" bgcolor="#BDDCF4">
                    团号
                </th>
                <th width="80px" bgcolor="#BDDCF4">
                    出团时间
                </th>
                <th width="60px" bgcolor="#BDDCF4">
                    人数
                </th>
                <th width="80px" bgcolor="#BDDCF4">
                    客户单位
                </th>
                <th width="80px" bgcolor="#BDDCF4">
                    责任销售
                </th>
                <th width="80px" bgcolor="#BDDCF4">
                    总收入
                </th>
                <th width="75px" bgcolor="#BDDCF4">
                    已收
                </th>
                <th width="60px" bgcolor="#BDDCF4">
                    未收
                </th>
            </tr>
            <cc1:CustomRepeater ID="crp_GetArrenarageList" runat="server">
                <ItemTemplate>
                    <tr class="even">
                        <td align="center">
                           <%#Eval("TourClassId").ToString()=="单项服务"?"单项服务" :Eval("RouteName")%>
                        </td>
                        <td height="30" align="center">
                            <%#Eval("TourNo")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourClassId").ToString()=="单项服务"?"" : Eval("LeaveDate","{0:yyyy-MM-dd}") %>
                        </td>
                        <td align="center">
                            <%#Eval("PeopleNumber")%>
                        </td>
                        <td align="center">
                            <%#Eval("BuyCompanyName")%>
                        </td>
                        <td align="center">
                            <%#Eval("SalerName")%>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal((decimal.Parse(Eval("HasCheckMoney").ToString()) + decimal.Parse(Eval("NotCheckMoney").ToString())))%></font>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%></font>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td align="center">
                             <%#Eval("TourClassId").ToString()=="单项服务"?"单项服务" :Eval("RouteName")%>
                        </td>
                        <td height="30" align="center">
                            <%#Eval("TourNo")%>
                        </td>
                        <td align="center">
                                    <%#Eval("TourClassId").ToString()=="单项服务"?"" : Eval("LeaveDate","{0:yyyy-MM-dd}") %>
                        </td>
                        <td align="center">
                            <%#Eval("PeopleNumber")%>
                        </td>
                        <td align="center">
                            <%#Eval("BuyCompanyName")%>
                        </td>
                        <td align="center">
                            <%#Eval("SalerName")%>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%# EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal((decimal.Parse(Eval("HasCheckMoney").ToString()) + decimal.Parse(Eval("NotCheckMoney").ToString())))%></font>
                        </td>
                        <td align="center">
                            <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%></font>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
            <tr class="even" id="tbl_ExportPage" runat="server">
                <td height="30" colspan="9" align="right" class="pageup">
                    <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
