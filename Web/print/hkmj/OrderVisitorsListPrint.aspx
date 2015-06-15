<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderVisitorsListPrint.aspx.cs"
    Inherits="Web.print.hkmj.OrderVisitorsListPrint" MasterPageFile="~/masterpage/Print.Master"
    Title="游客名单" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="c1" runat="server">
    <table width="759" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td height="50" align="center">
                <span style="font-size: 20px; font-weight: bold;">
                    <asp:Literal ID="litLeaveDate" runat="server"></asp:Literal>
                    <asp:Literal ID="litTrafficName" runat="server"></asp:Literal>
                    送机计划表</span>
            </td>
        </tr>
        <tr>
            <td>
                <table class="table_normal2" width="759" cellspacing="0" cellpadding="0" border="none"
                    align="center">
                    <tr>
                        <th width="8%" align="center" bgcolor="#FFFFFF">
                            客人类型
                        </th>
                        <th width="18%" align="center" bgcolor="#FFFFFF">
                            <strong>产品名称</strong>
                        </th>
                        <th width="20%" align="center" bgcolor="#FFFFFF">
                            <strong>组团社</strong>
                        </th>
                        <th width="6%" align="center" bgcolor="#FFFFFF">
                            <strong>人数</strong>
                        </th>
                        <th width="33%" align="center" bgcolor="#FFFFFF">
                            <strong>旅游姓名及联系方式(身份证)</strong>
                        </th>
                        <th width="15%" align="center" bgcolor="#FFFFFF">
                            <strong>备注</strong>
                        </th>
                    </tr>
                    <asp:Repeater ID="repVisitorsList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td align="center" bgcolor="#FFFFFF">
                                    &nbsp;<%# (EyouSoft.Model.EnumType.TourStructure.TourType)Eval("TourType")%>
                                </td>
                                <td align="center" bgcolor="#FFFFFF">
                                    &nbsp;<%# Eval("RouteName")%>
                                </td>
                                <td align="center" bgcolor="#FFFFFF">
                                    &nbsp;<%# Eval("BuyCompanyName")%>
                                </td>
                                <td align="center" bgcolor="#FFFFFF">
                                    &nbsp;<%# Eval("ChenRenShu")%>大<%# Eval("ErTongShu")%>小
                                </td>
                                <td align="left" bgcolor="#FFFFFF">
                                    &nbsp;<%# GetCustomerList(Eval("OrderCustomers")) %>
                                </td>
                                <td align="center" bgcolor="#FFFFFF">
                                    &nbsp;<%#GetTourOrderAmountPlusInfo( Eval("TourOrderAmountPlusInfo"))%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td align="right" bgcolor="#FFFFFF">
                            人数合计：
                        </td>
                        <td colspan="5" align="left" bgcolor="#FFFFFF">
                            <asp:Literal ID="litAdultNum" runat="server"></asp:Literal>
                            大
                            <asp:Literal ID="litChildRenNum" runat="server"></asp:Literal>
                            小=
                            <asp:Literal ID="litCountNum" runat="server"></asp:Literal>
                            人
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
