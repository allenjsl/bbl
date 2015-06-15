<%@ Page Title="团队结算单" Language="C#" AutoEventWireup="true" CodeBehind="TourTeamSettlePrint.aspx.cs"
    Inherits="Web.print.wh.TourTeamSettlePrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal2">
        <col />
        <col width="120px" />
        <col width="82" />
        <col width="82" />
        <col width="83" />
        <col width="51" />
        <col width="50" />
        <col width="100" />
        <col width="220" />
        <tr height="49">
            <td colspan="9" height="49" width="679" align="center" valign="top">
                望海假期团队结算单
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                收件单位
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtBuyerCName" runat="server" Style="border: none;" size="24" CssClass="nonelineTextBox"
                    Width="350px"></asp:TextBox>
            </td>
            <td>
                收件人
            </td>
            <td>
                <asp:TextBox ID="txtContact" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                电话
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtPbone" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
                传真
            </td>
            <td>
                <asp:TextBox ID="txtFax" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                线路
            </td>
            <td colspan="2">
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
            <tr style="height: 25px;" align="center">
                <td rowspan="<%=this.servicesCount+1 %>" colspan="2">
                    综合费用
                </td>
                <td colspan="1">
                    项目
                </td>
                <td colspan="2">
                    接待标准
                </td>
                <td colspan="2">
                    单价
                </td>
                <td colspan="1">
                    人数
                </td>
                <td colspan="1">
                    金额
                </td>
            </tr>
            <asp:Repeater ID="rptProject" runat="server">
                <ItemTemplate>
                    <tr align="center">
                        <td colspan="1">
                            <%#Eval("ServiceType").ToString()%>
                        </td>
                        <td colspan="2">
                            <%#Eval("Service")%>
                        </td>
                        <td colspan="2">
                            <input type="text" value="<%#Eval("SelfUnitPrice","{0:c}")%>" style="width: 90%"
                                class="nonelineTextBox" />
                        </td>
                        <td colspan="1">
                            <input type="text" value="<%#Eval("SelfPeopleNumber")%>" class="nonelineTextBox" />
                        </td>
                        <td colspan="1">
                            <span class="spanAddPrice">
                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("SelfPrice")).ToString("0.00"))%></span>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tr height="25">
            <td colspan="2" align="center">
                增减费用
            </td>
            <td colspan="2" align="center">
                添加
            </td>
            <td colspan="2" align="center">
                减少
            </td>
            <td colspan="3" align="center">
                备注
            </td>
        </tr>
        <asp:Repeater ID="rptFrist" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                    </td>
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceAddExpense")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="2">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("FinanceRedExpense")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <%#Eval("FinanceRemark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptSecound" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                    </td>
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="2">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <%#Eval("Remark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptThird" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                    </td>
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="2">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <%#Eval("FRemark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptForth" runat="server">
            <ItemTemplate>
                <tr height="25">
                    <td colspan="2">
                    </td>
                    <td colspan="2">
                        <span class="spanAddPrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("AddAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="2">
                        <span class="spanReducePrice">
                            <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("ReduceAmount")).ToString("0.00"))%></span>
                    </td>
                    <td colspan="3">
                        <%#Eval("Remark")%>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr height="25">
            <td colspan="2" height="25">
                费用合计
            </td>
            <td colspan="6">
                <asp:TextBox ID="txtALLPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                已付费用
            </td>
            <td colspan="6">
                <asp:TextBox ID="txtHasPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                未付费用
            </td>
            <td colspan="6">
                <asp:TextBox ID="txtNotPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="6" height="25">
                对公账号：
            </td>
            <td colspan="3">
                户名：张奔
            </td>
        </tr>
        <tr height="25">
            <td colspan="6" rowspan="2" height="50" width="309">
                户名：杭州西湖假期国际旅行社有限公司下城营业部
            </td>
            <td colspan="3">
                建设银行：6227 0015 4180 0293 354
            </td>
        </tr>
        <tr height="25">
            <td colspan="3" height="25">
                工商银行：622202 1202009792525
            </td>
        </tr>
        <tr>
            <td colspan="6" rowspan="2">
                开户行：交通银行杭州城北支行
            </td>
            <td colspan="3">
                农业银行：62284 8032 09784 20414
            </td>
        </tr>
        <tr height="25">
            <td colspan="3" class="style2">
                交通银行：622260 0170002887619
            </td>
        </tr>
        <tr height="25">
            <td colspan="6" rowspan="2" height="50">
                账号：331066100018010027612?
            </td>
            <td colspan="3">
                中国银行：4563511300109283647
            </td>
        </tr>
        <tr height="25">
            <td colspan="3" height="25">
                建行(宁波)：6227 0015 9550 0281 598
            </td>
        </tr>
        <tr>
            <td colspan="9" width="679" class="style1">
                请贵公司将尾款尽快打入以上账号，如无异议请盖章确认回传。谢谢! 祝：合作愉快，贵公司生意兴隆！
            </td>
        </tr>
        <tr>
            <td colspan="6" height="25">
                发件人：<asp:TextBox ID="txtSendMan" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
            <td colspan="3">
                手机：<asp:TextBox ID="txtTelPhone" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="6" height="25">
                电话：<asp:TextBox ID="txtMobel" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
            <td colspan="3">
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
