<%@ Page Title="行程单" Language="C#" AutoEventWireup="true" CodeBehind="FastRouteInfoPrint.aspx.cs"
    Inherits="Web.print.bbl.FastRouteInfoPrint" MasterPageFile="~/masterpage/Print.Master" %>

<asp:Content ContentPlaceHolderID="PrintC1" ID="Content1" runat="server">
    <table  align="center" width="759">
        <tbody>
            <tr>
                <td class="normaltd" height="30">
                    <table class="table_normal2" width="100%">
                        <tbody>
                            <tr>
                                <th height="25" align="center" colspan="3">
                                    线路名称：<asp:Label ID="lblAreaName" runat="server" Text=""></asp:Label>
                                </th>
                            </tr>
                            <tr>
                                <td  height="25" align="left">
                                    旅游天数：<asp:Label ID="lblDay" runat="server" Text=""></asp:Label>
                                </td>
                                <td  align="left" colspan="2">
                                    发布人：
                                    <asp:Label ID="lblAuthor" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td  height="25" align="left">
                                    上团数：<asp:Label ID="lblTourCount" runat="server" Text=""></asp:Label>
                                </td>
                                <td  align="left" colspan="2">
                                    收客数 ：<asp:Label ID="lblVisitorCount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td height="10"  align="center">
                </td>
            </tr>
            <tr>
                <td 　height="30" align="center">
                    <table class="table_normal2" align="center" width="100%">
                        <tbody>
                            <tr>
                                <td height="20" class="td_b_border" align="left">
                                    <b>· 行程安排</b>
                                </td>
                            </tr>
                            <tr>
                                <td   align="left">
                                    <asp:Literal ID="litTravel" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <th height="30"  align="left">
                      · 服务标准
                </th>
            </tr>
            <tr>
                <td height="30" style="border: 1px solid rgb(0, 0, 0);">
                    <asp:Literal ID="litService" runat="server"></asp:Literal>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
