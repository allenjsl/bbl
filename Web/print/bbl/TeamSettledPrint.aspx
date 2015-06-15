<%@ Page Title="�Ŷӽ��㵥" Language="C#" AutoEventWireup="true" CodeBehind="TeamSettledPrint.aspx.cs"
    Inherits="Web.print.bbl.TeamSettledPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="760px" align="center" class="table_normal">
        <tr>
            <td width="626" height="35" align="center" class="td_r_b_border">
                <span style="font-size: 20px; font-weight: bold;"><%=SiteUserInfo.SysId == 3 ? "�� �� �� �� ":" �� �� �� �� "%> �� �� �� ϸ ��</span>
            </td>
            <th width="170" align="center" class="td_r_b_border">
                ��ţ�<input type="text" id="txtNumber" class="underlineTextBox" />
            </th>
        </tr>
        <tr>
            <td colspan="2" align="left" class="normaltd" style="margin: 0; padding: 0;">
                <table width="100%" align="center" class="table_noneborder">
                    <tr>
                        <th height="25" colspan="2" align="center" class="td_r_b_border">
                            �� ��
                        </th>
                        <td colspan="2" align="center" class="td_r_b_border">
                            <asp:Label ID="lblTourCode" runat="server" Text=""></asp:Label>
                        </td>
                        <th width="9%" align="center" class="td_r_b_border">
                            �� · 
                        </th>
                        <td colspan="3" align="center" class="td_r_b_border">
                            <asp:Label ID="lblAreaName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th height="25" colspan="2" align="center" class="td_r_b_border">
                            ��ֹ����
                        </th>
                        <td colspan="2" align="center" class="td_r_b_border">
                            [<asp:Label ID="lblBenginDate" runat="server" Text=""></asp:Label>]-[<asp:Label ID="lblEndDate"
                                runat="server" Text=""></asp:Label>]
                        </td>
                        <th align="center" class="td_r_b_border">
                            �� ��
                        </th>
                        <td colspan="3" align="center" class="td_r_b_border">
                            <asp:Label ID="lblCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th width="3%" height="25" align="center" class="td_r_b_border">
                            ���
                        </th>
                        <th width="9%" align="center" class="td_r_b_border">
                            �� ��<br />
                            �� ��
                        </th>
                        <th align="center" class="td_r_b_border">
                            ������/��ϵ��
                        </th>
                        <th align="center" class="td_r_b_border">
                            �ֻ�/����
                        </th>
                        <th align="center" class="td_r_b_border">
                            С ��
                        </th>
                        <th width="12%" align="center" class="td_r_b_border">
                            δ�տ�
                        </th>
                        <th width="12%" align="center" class="td_r_b_border">
                            ���տ�
                        </th>
                        <th width="13%" align="center" class="td_r_b_border">
                            �� ע
                        </th>
                    </tr>
                    <asp:Repeater ID="rptCustomer" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="25" align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-7,8,rptCustomerTDCount) %>">
                                    <%#Container.ItemIndex+1 %>
                                </td>
                                <td style="margin: 0; padding: 0;" height="25" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-6,8,rptCustomerTDCount) %>">
                                    <table class="table_noneborder">
                                        <%#GetCustomerHtmlByList((System.Collections.Generic.IList<EyouSoft.Model.TourStructure.TourOrderCustomer>)Eval("CustomerList"))%>
                                    </table>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-5,8,rptCustomerTDCount) %>">
                                    <%#Eval("BuyCompanyName")%>/<%#Eval("ContactName")%>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-4,8,rptCustomerTDCount) %>">
                                    <%#Eval("ContactMobile")%>/<%#Eval("ContactFax")%>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-3,8,rptCustomerTDCount) %>">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("FinanceSum")))%>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-2,8,rptCustomerTDCount) %>">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("NotReceived")))%>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-1,8,rptCustomerTDCount) %>">
                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("HasCheckMoney")))%>
                                </td>
                                <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*8-7,8,rptCustomerTDCount) %>">
                                    <%#Eval("SpecialContent")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <th height="25" colspan="2" align="center" class="td_t_r_border" style="border-bottom: solid 1px;">
                            �ſ��ܼ�
                        </th>
                        <th align="center" class="td_r_b_border" style="border-top: solid 1px;">
                            �� Ʊ ��
                        </th>
                        <th colspan="2" align="center" class="td_r_b_border" style="border-top: solid 1px;">
                            �� �� ��
                        </th>
                        <th colspan="2" align="center" class="td_r_b_border" style="border-top: solid 1px;">
                            �� ע
                        </th>
                        <th align="center" class="td_r_b_border" style="border-top: solid 1px;">
                            �� �� ��
                        </th>
                    </tr>
                    <tr>
                        <th colspan="2" align="center" class="td_r_b_border">
                            <asp:Label ID="lblTeamPrice" runat="server" Text=""></asp:Label>
                        </th>
                        <td align="center" width="245px" valign="top" class="td_r_b_border" style="margin: 0;
                            padding: 0;">
                            <table class="table_noneborder" width="100%" id="table_ticket">
                                <tr>
                                    <td class="td_r_b_border">
                                        <b>����</b>
                                    </td>
                                    <td class="td_r_b_border">
                                        <b>�ۿ�</b>
                                    </td>
                                    <td class="td_b_border">
                                        <b>���</b>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptTicketPrice" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*3-2,3,rptTicketPriceTDCount) %>">
                                                <%#GetPeopleCount(Eval("FundAdult"), Eval("FundChildren"))%>
                                            </td>
                                            <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*3-1,3,rptTicketPriceTDCount) %>">
                                                <%#GetDiscount(Eval("TicketFlights"))%>
                                            </td>
                                            <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*3,3,rptTicketPriceTDCount) %>">
                                                <%#EyouSoft.Common.Utils.FilterEndOfTheZeroString(Convert.ToDecimal(Eval("TotalAmount")).ToString("0.00"))%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                        <td align="center" colspan="2" class="td_r_b_border" style="margin: 0; padding: 0;">
                            <table class="table_noneborder" width="100%" id="table_dj">
                                <tr>
                                    <td class="td_r_b_border">
                                        <b>�ؽ���</b>
                                    </td>
                                    <td class="td_b_border">
                                        <b>С ��</b>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptDjPrice" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*2-1,2,rptDjPriceTDCount) %>">
                                                <%#Eval("LocalTravelAgency")%>
                                            </td>
                                            <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*2,2,rptDjPriceTDCount) %>">
                                                <a class="class_a">
                                                    <%#EyouSoft.Common.Utils.FilterEndOfTheZeroDecimal(Convert.ToDecimal(Eval("Settlement")))%></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td class="td_t_r_border">
                                        �ܼƣ�
                                    </td>
                                    <td class="td_t_border">
                                        <span id="spanAllPrice"></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="2" align="center" class="td_r_b_border">
                            <asp:Label ID="lblRemarks" runat="server" Text=""></asp:Label>
                        </td>
                        <td align="center" class="td_r_b_border">
                            <asp:Label ID="lblProfit" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th height="25" align="center" class="td_r_border">
                            �Ƶ�
                        </th>
                        <td height="25" class="td_r_border">
                            <asp:Label ID="lblMeter" runat="server" Text=""></asp:Label>
                        </td>
                        <th align="center" class="td_r_border">
                            Ʊ ��
                        </th>
                        <td align="center" class="td_r_border">
                            <asp:Label ID="lblTicket" runat="server" Text=""></asp:Label>
                        </td>
                        <th align="center" class="td_r_border">
                            �� ��
                        </th>
                        <td align="center" class="td_r_border">
                            &nbsp;
                        </td>
                        <th align="center" class="td_r_border">
                            �� ��
                        </th>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td height="30" align="right" class="normaltd">
                ����һ�� ��� �ڶ��� �Ƶ� ������ ����
            </td>
            <td align="left" class="normaltd">
                &nbsp;
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        $(function() {
        PrintMaster.AdaptiveHeight("table_ticket");
        PrintMaster.AdaptiveHeight("table_dj");
        
        
            var allPrice = 0;
            $(".class_a").each(function() {
                var price = parseFloat($(this).html());
                if (price != NaN && price > 0) {
                    allPrice += price;
                }
            })
            allPrice = parseInt(allPrice * 100) / 100;
            $("#spanAllPrice").html(allPrice);
        })
    </script>

</asp:Content>
