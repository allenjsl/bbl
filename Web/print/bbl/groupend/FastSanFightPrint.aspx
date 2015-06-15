<%@ Page Title="行程单" Language="C#" AutoEventWireup="true" CodeBehind="FastSanFightPrint.aspx.cs" Inherits="Web.print.bbl.groupend.FastSanFightPrint" MasterPageFile="~/masterpage/Print.Master"%>
<asp:Content ID="Content2" ContentPlaceHolderID="PrintC1" runat="server">
<table class="table_noneborder" align="center" width="759">
        <tbody>
            <tr>
                <td height="30">
                    <table class="table_normal2" width="100%">
                        <tbody>
                            <tr>
                                <td height="25" align="center" colspan="3">
                                    线路名称：<asp:Label ID="lblAreaName" runat="server" Text=""></asp:Label> 
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="left" width="40%">
                                    团 号：<asp:Label ID="lblTeamNum" runat="server" Text=""></asp:Label>
                                    </td>
                                <td  align="center" width="30%">
                                    旅游天数：<asp:Label ID="lblDay" runat="server" Text=""></asp:Label>
                                </td>
                                <td align="center" width="30%">
                                    人 数： <asp:Label ID="lblAdult" runat="server" Text=""></asp:Label>+<asp:Label ID="lblChild" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td height="25" align="left">
                                    出发交通：<asp:Label ID="lblBegin" runat="server" Text=""></asp:Label>
                                </td>
                                <td align="left" colspan="2">
                                    返程交通 ：<asp:Label ID="lblEnd" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10" align="center">
                </td>
            </tr>
            <tr>
                <td height="30" align="center">
                    <table class="table_normal2" align="center" width="100%">
                        <tbody>
                            <tr>
                                <td height="20" align="left">
                                    <b>·行程安排</b>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Literal ID="litTravel" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="30" align="left">
                    ·服务标准
                </td>
            </tr>
            <tr>
                <td height="30" style="border:1px solid #000">
                   <asp:Literal ID="litService" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td height="30" align="left">
                    ·地接社信息
                </td>
            </tr>
            <tr>
                <td height="30">
                    <table class="table_normal2" align="center" width="100%">
                        <tbody>
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
                                        <td height="20" align="center" width="33%">
                                            <%#Eval("Name") %>
                                        </td>
                                        <td height="20" align="center" width="33%">
                                            <%#Eval("LicenseNo") %>
                                        </td>
                                        <td height="20" align="center" width="33%">
                                           <%#Eval("Telephone") %>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
