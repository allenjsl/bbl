<%@ Page Title="ɢƴ���㵥" Language="C#" AutoEventWireup="true" CodeBehind="TeamSettledPrint.aspx.cs"
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
                ���������Ŷӽ��㵥
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                <nobr>�ռ���λ</nobr>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtBuyerCName" runat="server" Style="border: none;" size="24" CssClass="nonelineTextBox"
                    Width="350px"></asp:TextBox>
            </td>
            <td colspan="2">
                <nobr>�ռ���</nobr>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtContact" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                �绰
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtPbone" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td colspan="2">
                ����
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtFax" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                ��·
            </td>
            <td> 
                <asp:TextBox ID="txtAreaName" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
                ����
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtCount" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
                ����
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
                    �ۺϷ���
                </td>
                <td align="center">
                    ���˼�
                </td>
                <td align="center">
                    ������
                </td>
                <td align="center" colspan="2">
                    ��ͯ��
                </td>
                <td align="center">
                    <nobr>��ͯ��</nobr>
                </td>
                <td align="center">
                    �ϼ�
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
                ��������
            </td>
            <td colspan="2" align="center">
                ���
            </td>
            <td colspan="3" align="center">
                ����
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
                ���úϼ�
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtALLPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                �Ѹ�����
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtHasPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="2" height="25">
                δ������
            </td>
            <td colspan="5">
                <asp:TextBox ID="txtNotPrice" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                �Թ��˺ţ�
            </td>
            <td colspan="4">
                �������ű�
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" rowspan="2" height="50" width="309">
                �����������������ڹ������������޹�˾�³�Ӫҵ��
            </td>
            <td colspan="4">
                �������У�6227 0015 4180 0293 354
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                �������У�622202 1202009792525
            </td>
        </tr>
        <tr>
            <td colspan="4" rowspan="2">
                �����У���ͨ���к��ݳǱ�֧��
            </td>
            <td colspan="4">
                ũҵ���У�62284 8032 09784 20414
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" class="style2">
                ��ͨ���У�622260 0170002887619
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" rowspan="2" height="50">
                �˺ţ�331066100018010027612?
            </td>
            <td colspan="4">
                �й����У�4563511300109283647
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                ����(����)��6227 0015 9550 0281 598
            </td>
        </tr>
        <tr>
            <td colspan="8" width="679" class="style1">
                ���˾��β�����������˺ţ��������������ȷ�ϻش���лл! ף��������죬��˾������¡��
            </td>
        </tr>
        <tr>
            <td colspan="4" height="25">
                �����ˣ�<asp:TextBox ID="txtSendMan" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
            <td colspan="4">
                �ֻ���<asp:TextBox ID="txtTelPhone" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
        </tr>
        <tr height="25">
            <td colspan="4" height="25">
                �绰��<asp:TextBox ID="txtMobel" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
            </td>
            <td colspan="4">
                ���棺<asp:TextBox ID="txtFaxSecound" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
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
