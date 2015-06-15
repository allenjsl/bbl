<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetEmpTourList.aspx.cs"
    Inherits="Web.StatisticAnalysis.EmployeeAchievementsTime.GetEmpTourList" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>
<%@ Register Assembly="ControlLibrary" Namespace="ControlLibrary" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>人数</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="800" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
            <tr class="odd">
                <th width="130" height="30" align="center">
                    线路名称
                </th>
                <th width="130" align="center">
                    团号
                </th>
                <th width="130" align="center">
                    出团日期
                </th>
                <th width="130" align="center">
                    供应商
                </th>
                <th width="130" align="center">
                    人数
                </th>
                <th width="130" align="center">
                    金额
                </th>
            </tr>
            <cc1:CustomRepeater ID="crp_GetEmpTourList" runat="server">
                <ItemTemplate>
                    <tr class="even">
                        <td height="30" align="center">
                            <%#Eval("TourClassId").ToString() == "单项服务" ? "单项服务" : Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourNo")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourClassId").ToString() == "单项服务" ? "" :Eval("LeaveDate","{0:yyyy-MM-dd}") %>
                        </td>
                        <td align="center">
                            <%#Eval("buycompanyname") %>
                        </td>
                        <td align="center">
                            <%#Eval("PeopleNumber") %>
                        </td>
                        <td align="center">
                            <font class="fred">
                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="odd">
                        <td height="30" align="center">
                            <%#Eval("RouteName")%>
                        </td>
                        <td align="center">
                            <%#Eval("TourNo")%>
                        </td>
                        <td align="center">
                            <%#Eval("LeaveDate","{0:yyyy-MM-dd}") %>
                        </td>
                        <td align="center">
                            <%#Eval("buycompanyname")%>
                        </td>
                        <td align="center">
                            <%#Eval("PeopleNumber") %>
                        </td>
                        <td align="center">
                            <font class="fred">
                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </cc1:CustomRepeater>
        </table>
        <table width="100%" id="tbl_ExPageEmp" runat="server" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td align="right">
                    <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
