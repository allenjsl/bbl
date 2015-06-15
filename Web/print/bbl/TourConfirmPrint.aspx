<%@ Page Title="团 队 报 价 单" Language="C#" MasterPageFile="~/masterpage/Print.Master"
    AutoEventWireup="true" CodeBehind="TourConfirmPrint.aspx.cs" Inherits="Web.print.bbl.TourConfirmPrint" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PrintC1" runat="server">
    <table class="table_normal" align="center" width="759">
        <tbody>
            <tr>
                <td height="30" align="center" class="normaltd">
                    <span style="font-size: 20px; font-weight: bold;"><%=SiteUserInfo.SysId == 3 ? "诚 信 假 期 ":" 我 爱 中 华 "%>团 队 报 价 单</span>
                </td>
            </tr>
            <tr style="display: none;">
                <td height="35" align="right" class="normaltd">
                    报团人：
                    <input type="text" id="textfield" name="textfield" class="underlineTextBox" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp; 报团日期：
                    <input type="text" id="textfield" name="textfield" class="underlineTextBox" />
                </td>
            </tr>
            <tr>
                <td align="center" class="normaltd" style="margin: 0; padding: 0">
                    <table class="table_noneborder" align="center" width="100%">
                        <tbody>
                            <tr>
                                <td align="center" style="border-right:1px solid #000;border-bottom:1px solid #000" width="15%">
                                    组 团 社
                                </td>
                                <td align="center" class="td_r_b_border" width="25%" colspan="4">
                                    <asp:TextBox ID="txtZutuanName" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                                <td align="center" class="td_r_b_border" width="15%">
                                    收件人
                                </td>
                                <td align="center" class="td_r_b_border" width="15%">
                                    <asp:TextBox ID="txtIncomeMan" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                                <td align="center" class="td_r_b_border" width="15%">
                                    传&nbsp; 真
                                </td>
                                <td align="center" class="td_b_border"  width="15%">
                                    <asp:TextBox ID="txtFax" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="td_r_b_border" >
                                    出团日期
                                </td>
                                <td align="center" class="td_r_b_border"  colspan="4">
                                    <input id="Text1" type="text" class="nonelineTextBox" value='<%=LeaveDate%>' />
                                </td>
                                <td align="center" class="td_r_b_border">
                                    发件人
                                </td>
                                <td align="center" class="td_r_b_border" >
                                    <input id="Text2" type="text" class="nonelineTextBox" />
                                </td>
                                <td align="center" class="td_r_b_border" >
                                    传&nbsp; 真
                                </td>
                                <td align="center" class="td_b_border" >
                                    <input id="Text3" type="text" class="nonelineTextBox" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="td_r_b_border" >
                                    发件日期
                                </td>
                                <td align="center" class="td_r_b_border"  colspan="4">
                                    1 月20日
                                </td>
                                <td align="center" class="td_r_b_border" >
                                    人&nbsp; 数
                                </td>
                                <td align="center" class="td_r_b_border" >
                                    <asp:Label ID="lblAdult" runat="server" Text=""></asp:Label>
                                </td>
                                <td align="center" class="td_r_b_border" >
                                    团队/散客
                                </td>
                                <td align="center" class="td_b_border" >
                                    <asp:TextBox ID="txtTourType" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="table_r_border" >
                                    集合时间
                                </td>
                                <td align="center" class="table_r_border" >
                                    <asp:Label ID="lblGatheredDate" runat="server" Text=""></asp:Label>
                                </td>
                                <td align="center" class="table_r_border">
                                    送团
                                </td>
                                <td align="center" class="table_r_border" >
                                    <asp:TextBox ID="txtOffName" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                                <td align="center" class="table_r_border" >
                                    电话
                                </td>
                                <td align="center" class="table_r_border" >
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                                </td>
                                <td align="center"  colspan="3">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="nonelineTextBox" Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="table_r_border" style="margin: 0; padding: 0">
                    <table class="table_noneborder"  align="center" width="100%">
                        <tbody>
                            <tr>
                                <td class="td_r_b_border" align="center" width="67">
                                    日 期
                                </td>
                                <td class="td_r_b_border" align="center" width="101">
                                    交 &nbsp; 通
                                </td>
                                <td class="td_r_b_border" align="center" width="462">
                                    行 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 程
                                </td>
                                <td class="td_r_b_border" align="center" width="49">
                                    用餐
                                </td>
                                <td align="center" class="td_b_border" width="49">
                                    住宿
                                </td>
                            </tr>
                            <asp:Repeater ID="rptTravel" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td height="25" align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*5-4,5,tdCount) %>">
                                            <%#GetDateByIndex(Container.ItemIndex)%>
                                        </td>
                                        <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*5-3,5,tdCount) %>">
                                            <%#GetTraffic(Container.ItemIndex+1)%>
                                        </td>
                                        <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*5-2,5,tdCount) %>" align="left">
                                            <%#Eval("Plan")%>
                                        </td>
                                        <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*5-1,5,tdCount) %>" align="center">
                                            <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                        </td>
                                        <td class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*5,5,tdCount) %>" align="center">
                                            <%#Eval("Hotel")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="normaltd">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td height="30" class="normaltd"  align="left">
                    <strong>报价已含费用及服务标准：</strong>
                    <asp:Repeater ID="rptProject" runat="server">
                        <ItemTemplate>
                            <%#Container.ItemIndex+1 %>、<strong>【
                                <%#Eval("ServiceType").ToString()%>
                                】</strong><%#Eval("Service")%>"
                        </ItemTemplate>
                    </asp:Repeater>
                    <br />
                    <br />
                    <strong>报价不含费用及服务</strong>：<asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                    <br />
                    <br />
                    <strong>购物安排：</strong><asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                    <br />
                    <br />
                    4、<strong>注意事项：</strong>&nbsp;
                    <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="normaltd">
                    &nbsp;
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
