<%@ Page Title="行程单" Language="C#" AutoEventWireup="true" CodeBehind="StandardSanFightPrint.aspx.cs" Inherits="Web.print.normal.StandardSanFightPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="759" align="center" class="table_noneborder">
        <tr>
            <td height="30" style="padding: 0px; margin: 0px">
                <table width="100%" class=" table_normal2">
                    <tr>
                        <th height="25" colspan="3" align="center">
                            线路名称：<asp:Label ID="lblAreaName" runat="server" Text=""> </asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td width="40%" height="25" align="left">
                            团 号：
                            <asp:Label ID="lblTeamNum" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="txttourcode" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td width="30%" align="left">
                            旅游天数：<asp:Label ID="lblDayCount" runat="server" Text=""></asp:Label>天
                        </td>
                        <td width="30%" align="left">
                            人 数：
                            <asp:Label ID="lblAdult" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="TXTADULT" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="left">
                            出发交通：
                            <asp:Label ID="lblBegin" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="TXtBEGIN" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                        <td colspan="2" align="left">
                            返程交通 ：
                            <asp:Label ID="lblEnd" runat="server" Text=""></asp:Label>
                            <asp:TextBox ID="TXTEnd" runat="server" CssClass="nonelineTextBox"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="40" align="left" class="normaltd">
                • 行程安排
            </th>
        </tr>
        <tr>
            <td height="30" style="padding: 0px; margin: 0px">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="15%" height="25" align="center">
                            日期
                        </td>
                        <td width="65%" align="center">
                            行程描述
                        </td>
                        <td width="10%" align="center">
                            住宿
                        </td>
                        <td width="10%" align="center">
                            用餐
                        </td>
                    </tr>
                    <asp:Repeater ID="rptTravel" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="25" align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4-3,4,rptTravelTDCount) %>">
                                    <%#GetDateByIndex(Container.ItemIndex)%>
                                </td>
                                <td align="left" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4-2,4,rptTravelTDCount) %>">
                                    <%#TextToHtml(Eval("Plan").ToString())%>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4-1,4,rptTravelTDCount) %>">
                                    <%#Eval("Hotel")%>
                                </td>
                                <td align="center" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*4,4,rptTravelTDCount) %>">
                                    <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left">
                • 服务标准
            </th>
        </tr>
        <tr>
            <td height="30" style="margin: 0; padding: 0;">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="15%" height="25" align="center">
                            包含项目：
                        </td>
                        <td style="margin: 0; padding: 0; border-bottom:solid 0px">
                            <table width="100%" class="table_noneborder">
                                <tr>
                                    <td width="82%" align="center" style=" border-right:solid 0px ">
                                        <b>接待标准</b>
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptProject" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td width="82%" style=" border-right:solid 0px ">
                                                <%#TextToHtml(Eval("Service").ToString())%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            不含项目：
                        </td>
                        <td class="td_b_border">
                            <asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            购物安排：
                        </td>
                        <td class="td_b_border">
                            <asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            自费项目：
                        </td>
                        <td align="left" class="td_b_border">
                            <asp:Label ID="lblSelfProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            儿童安排：
                        </td>
                        <td class="td_b_border">
                            <asp:Label ID="lblChildPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center">
                            注意事项：
                        </td>
                        <td align="left">
                            <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left">
                • 温馨提醒
            </th>
        </tr>
        <tr>
            <td height="30">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td>
                            <asp:Label ID="lblTips" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:Panel ID="Panel1" runat="server">
            <tr>
                <th height="30" align="left">
                    ·地接社信息
                </th>
            </tr>
            <tr>
                <td style="margin: 0; padding: 0">
                    <table align="center" width="100%" class="table_normal2">
                        <tr>
                            <td height="20" align="center" width="33%">
                                <b>地社名称</b>
                            </td>
                            <td height="20" align="center" width="33%">
                                <b>许可证号</b>
                            </td>
                            <td height="20" align="center" width="33%">
                                <b>电话</b>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptDjInfo" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td height="20" align="center" width="33%" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*3-2,3,rptDjInfoTDCount) %>">
                                        <%#Eval("Name") %>
                                    </td>
                                    <td height="20" align="center" width="33%" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*3-1,3,rptDjInfoTDCount) %>">
                                        <%#Eval("LicenseNo") %>
                                    </td>
                                    <td height="20" align="center" width="33%" class="<%# EyouSoft.Common.Utils.GetTdClassNameInNestedTableByIndex((Container.ItemIndex+1)*3,3,rptDjInfoTDCount) %>">
                                        <%#Eval("Telephone") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
        </asp:Panel>
    </table>
</asp:Content>
