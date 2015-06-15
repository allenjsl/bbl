<%@ Page Title="行程单" Language="C#" AutoEventWireup="true" CodeBehind="StandardTourInfoPrint.aspx.cs" Inherits="Web.print.normal.StandardTourInfoPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
    <table width="760px" align="center" class="table_noneborder">
        <tr>
            <td height="30" class="normaltd" style="padding: 0; margin: 0;">
                <table width="100%" border="0" class="table_normal2">
                    <tr>
                        <th height="25" colspan="3" align="center" >
                            线路名称：<asp:Label ID="lblAreaName" runat="server" Text=""> </asp:Label>
                        </th>
                    </tr>
                    <tr>
                        <td width="40%" height="25" align="left" >
                            团 号：<asp:Label ID="lblTeamNum" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="30%" align="left" >
                            旅游天数：<asp:Label ID="lblDayCount" runat="server" Text=""></asp:Label>
                        </td>
                        <td width="30%" align="left" >
                            人 数：
                            <asp:Label ID="lblAdult" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="left" >
                            出发交通：<asp:Label ID="lblBegin" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="2" align="left">
                            返程交通：<asp:Label ID="lblEnd" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="40" align="left" >
                • 行程安排
            </th>
        </tr>
        <tr>
            <td height="30" style="padding: 0; margin: 0;">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="15%" height="25" align="center" >
                            日期
                        </td>
                        <td width="65%" align="center" >
                            行程描述
                        </td>
                        <td width="10%" align="center" >
                            住宿
                        </td>
                        <td width="10%" align="center" >
                            用餐
                        </td>
                    </tr>
                    <asp:Repeater ID="rptTravel" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td height="25" align="center" >
                                    <%#GetDateByIndex(Container.ItemIndex)%>
                                </td>
                                <td align="left" >
                                    <%#Eval("Plan")%>
                                </td>
                                <td align="center" >
                                    <%#Eval("Hotel")%>
                                </td>
                                <td align="center" >
                                    <%#GetDinnerByValue(Eval("Dinner").ToString())%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left" class="normaltd">
                • 服务标准
            </th>
        </tr>
        <tr>
            <td height="30" style="padding: 0; margin: 0;">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td width="15%" height="25" align="center" >
                            包含项目：
                        </td>
                        <td align="left" >
                            <asp:Repeater ID="rptProject" runat="server">
                                <ItemTemplate>
                                    <%#Eval("Service")%>
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" >
                            不含项目：
                        </td>
                        <td >
                            <asp:Label ID="lblNoProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" >
                            购物安排：
                        </td>
                        <td >
                            <asp:Label ID="lblBuy" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" >
                            自费项目：
                        </td>
                        <td >
                            <asp:Label ID="lblSelfProject" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" >
                            儿童安排：&nbsp;
                        </td>
                        <td align="left" >
                            <asp:Label ID="lblChildPlan" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td height="25" align="center" >
                            注意事项：
                        </td>
                        <td align="left" >
                            <asp:Label ID="lblNote" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th height="30" align="left" >
                • 温馨提醒
            </th>
        </tr>
        <tr>
            <td height="30">
                <table width="100%" class="table_normal2">
                    <tr>
                        <td><asp:Label ID="lblTips" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
                
            </td>
        </tr>
        <tr>
            <th height="30" align="left" class="normaltd">
                ·地接社信息
            </th>
        </tr>
        <tr>
            <td height="30" style="margin: 0; padding: 0">
                <table border="0" align="center" width="100%" class="table_normal2">
                    <tbody>
                        <tr>
                            <td height="20" align="center" width="33%" >
                                <b>地社名称</b>
                            </td>
                            <td height="20" align="center" width="33%">
                                <b>许可证号</b>
                            </td>
                            <td height="20" align="center" width="33%" >
                                <b>电话</b>
                            </td>
                        </tr>
                        <asp:Repeater ID="rptDjInfo" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td height="20" align="center" width="33%" >
                                        <%#Eval("Name") %>
                                    </td>
                                    <td height="20" align="center" width="33%" >
                                        <%#Eval("LicenseNo") %>
                                    </td>
                                    <td height="20" align="center" width="33%" >
                                        <%#Eval("Telephone") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
