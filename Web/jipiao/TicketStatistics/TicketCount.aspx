<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketCount.aspx.cs" Inherits="Web.jipiao.TicketStatistics.TicketCount" %>
<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExportPageSet" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
         .FlightTD{ border-top:solid 1px #fff; border-right:solid 1px #fff; text-align:center; height:30;}
     </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="900" style="margin: 10px;">
            <tbody>
                <tr class="odd">
                    <th height="30" bgcolor="#bddcf4" width="25">
                        编号
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        航空公司
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        航段
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                                                日期
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                                                航班号/时间
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        PNR
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        票号
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        票面价<br />
                        成人/儿童
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        税/机建
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        代理费
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        票款金额
                    </th>
                    <th bgcolor="#bddcf4" width="67">
                        出票人数
                    </th>
                </tr>
                <asp:Repeater ID="rptList" runat="server">
                <ItemTemplate>
                      <tr class="even">
                    <td height="30" align="center">
                         <%#Container.ItemIndex+1 %>
                    </td>
                    <td colspan="4">
                      <table width="100%" border="1" style="border-collapse:collapse; border:solid 1px #fff;"><%#GetTdByAreaCompany(Eval("TicketFlightList"))%></table>
                    </td>
                    
                    <td align="center" >
                       <%#Eval("PNR") %>
                    </td>
                   
                    <td align="center" >
                       <%#Eval("TicketNum")%>
                    </td>
                   <%#GetTdTicketInfo(Eval("TicketKindList"))%>
                </tr>
                </ItemTemplate>
                </asp:Repeater>
              
       
                <tr class="even">
                    <td height="30" align="right" class="pageup" colspan="12">
                          <cc2:ExportPageInfo ID="ExportPageInfo1" runat="server" LinkType="4" PageStyleType="NewButton"
                                CurrencyPageCssClass="RedFnt" />
                               <asp:Label ID="lblMsg" runat="server" Text="未找到数据!"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
