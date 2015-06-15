<%@ Page Title="散拼结算单" Language="C#" AutoEventWireup="true" CodeBehind="TeamSettledPrint.aspx.cs"
    Inherits="Web.print.wh.TeamSettledPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal2">
        <col width="43" />
        <col width="50" />
        <col width="82" />
        <col width="83" />
        <col width="51" />
        <col width="50" />
        <col width="100" />
        <col width="220" />
        <tr height="49">
            <td colspan="8" height="49" width="679" align="center" valign="top">
                望海假期团队结算单
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                <nobr>收件单位</nobr>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtBuyerCName" runat="server" Style="border: none;" size="24" CssClass="nonelineTextBox"
                    Width="350px"></asp:TextBox>
            </td>
            <td colspan="2">
                <nobr>收件人</nobr>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtContact" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                电话
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtPbone" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td colspan="2">
                传真
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtFax" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                线路
            </td>
            <td> 
                <asp:TextBox ID="txtAreaName" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
                人数
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtCount" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
                日期
            </td>
            <td>
                <asp:TextBox ID="txtDataTime" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tbody id="tbMoney" runat="server">
        </tbody>
        <tbody id="tbMoneySP" runat="server" visible="false">
            <tr>
                <td colspan="2">
                    综合费用
                </td>
                <td align="center">
                    成人价
                </td>
                <td align="center">
                    成人数
                </td>
                <td align="center" colspan="2">
                    儿童价
                </td>
                <td align="center">
                    <nobr>儿童数</nobr>
                </td>
                <td align="center">
                    合计
                </td>
            </tr>
            <asp:Repeater runat="server" ID="rpt_orderList">
                <ItemTemplate>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td align="center">
                            <%#Eval("PersonalPrice","{0:c}")%>
                        </td>
                        <td align="center">
                            <%#Eval("AdultNumber")%>
                        </td>
                        <td colspan="2" align="center">
                            <%#Eval("ChildPrice", "{0:c}")%>
                        </td>
                        <td align="center">
                            <%#Eval("ChildNumber")%>
                        </td>
                        <td align="center">
                            <%#sum(Eval("PersonalPrice"), Eval("AdultNumber"), Eval("ChildPrice"), Eval("ChildNumber"))%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tr height="25">
            <td rowspan="<%=this.allCount+1 %>" colspan="2" style="width: 54px">
                增减费用
            </td>
            <td colspan="2" align="center">
                添加
            </td>
            <td colspan="3" align="center">
                减少
            </td>
            <td>
            </td>
        </tr>
        <asp:Repeater ID="rptFrist" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceAddExpense")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceRedExpense")).ToString("0.00"))%></span>
                    </td>
                    <td>
                        <%#Eval("FinanceRemark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptSecound" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%></span>
                    </td>
                    <td>
                        <%#Eval("Remark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptThird" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%></span>
                    </td>
                    <td>
                        <%#Eval("FRemark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptForth" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%></span>
                    </td>
                    <td>
                        <%#Eval("Remark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr height="25">
            <td colspan="2" height="25">
                费用合计
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtALLPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                已付费用
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtHasPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                未付费用
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtNotPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                对公账号：
            </td>
            <td colspan="4">
                户名：张奔
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" rowspan="2" height="50" width="309">
                户名：杭州西湖假期国际旅行社有限公司下城营业部
            </td>
            <td colspan="4">
                建设银行：6227 0015 4180 0293 354
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                工商银行：622202 1202009792525
            </td>
        </tr>
        <tr>
            <td colspan="4" rowspan="2">
                开户行：交通银行杭州城北支行
            </td>
            <td colspan="4">
                农业银行：62284 8032 09784 20414
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" class="style2">
                交通银行：622260 0170002887619
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" rowspan="2" height="50">
                账号：331066100018010027612?
            </td>
            <td colspan="4">
                中国银行：4563511300109283647
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                建行(宁波)：6227 0015 9550 0281 598
            </td>
        </tr>
        <tr>
            <td colspan="8" width="679" class="style1">
                请贵公司将尾款尽快打入以上账号，如无异议请盖章确认回传。谢谢! 祝：合作愉快，贵公司生意兴隆！
            </td>
        </tr>
        <tr>
            <td colspan="4" height="25">
                发件人：<asp:TextBox ID="txtSendMan" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
            <td colspan="4">
                手机：<asp:TextBox ID="txtTelPhone" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                电话：<asp:TextBox ID="txtMobel" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
            <td colspan="4">
                传真：<asp:TextBox ID="txtFaxSecound" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function() {
            var addPrice = 0;
            var reducePrice = 0;
            $(".spanAddPrice").each(function() {
                if (parseFloat($(this).html()) > 0) {
                    addPrice += parseFloat($(this).html());
                }
            });
            $(".spanReducePrice").each(function() {
                if (parseFloat($(this).html()) > 0) {
                    reducePrice += parseFloat($(this).html());
                }
            })
            $("#<%=txtALLPrice.ClientID %>").val(addPrice - reducePrice);
        })
    </script>

</asp:Content>
