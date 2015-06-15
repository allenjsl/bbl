<%@ Page Title="" Language="C#" MasterPageFile="~/masterpage/Print.Master" AutoEventWireup="true"
    CodeBehind="IncomeAreaPrint.aspx.cs" Inherits="Web.StatisticAnalysis.IncomeAccount.IncomeAreaPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="100%" id="tbl_UnIncomeAreaStaList" border="1" cellpadding="0" cellspacing="1">
        <tr>
            <th width="13%" align="center">
                线路名称
            </th>
            <th width="13%" align="center">
                团号
            </th>
            <th width="13%" align="center">
                出团日期
            </th>
            <th width="13%" align="center">
                客户单位
            </th>
            <th align="center">
                联系电话
            </th>
            <th width="13%" align="center">
                人数
            </th>
            <th width="13%" align="center">
                责任销售
            </th>
            <th width="13%" align="center">
                总收入
            </th>
            <th width="13%" align="center">
                已收
            </th>
            <th width="13%" align="center">
                未收
            </th>
        </tr>
        <asp:Repeater ID="repList" runat="server">
            <ItemTemplate>
                <tr>
                    <td align="center">
                        <%#Eval("tourclassid").ToString()%>
                    </td>
                    <td align="center">
                        <%#Eval("TourNo")%>
                    </td>
                    <td align="center">
                        <%#GetRouteName("1", Convert.ToString(Eval("RouteName")), Convert.ToString(Eval("tourclassid")), Convert.ToString(Eval("LeaveDate", "{0:yyyy-MM-dd}")))%>
                    </td>
                    <td align="center">
                        <%#Eval("BuyCompanyName")%>
                    </td>
                    <td align="center">
                        <%#Eval("ContactTel")%>
                    </td>
                    <td align="center">
                        <%#Eval("PeopleNumber")%>
                    </td>
                    <td align="center">
                        <%#Eval("SalerName")%>
                    </td>
                    <td align="center">
                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%></font>
                    </td>
                    <td align="center">
                        <font class="fred">￥
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("HasCheckMoney")))%></font>
                    </td>
                    <td align="center">
                        <font class="fred">￥<%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%></font>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr>
            <td rowspan="2" align="center">
                <b>总计:</b>
            </td>
            <td colspan="3" align="center">
                <b>总收入</b>
            </td>
            <td colspan="3" align="center">
                <b>已收</b>
            </td>
            <td colspan="3" align="center">
                <b>未收</b>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoneny)%>
            </td>
            <td colspan="3" align="center">
                ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(hasGetMoney) %>
            </td>
            <td colspan="3" align="center">
                ￥<%=EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(sumMoneny-hasGetMoney) %>
            </td>
        </tr>
    </table>
</asp:Content>
