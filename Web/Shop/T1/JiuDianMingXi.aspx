<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiuDianMingXi.aspx.cs" Inherits="Web.Shop.T1.JiuDianMingXi" MasterPageFile="~/Shop/T1/Default.Master" Title="酒店明细" %>
<%--酒店明细--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>
<asp:Content ID="PageHeader" runat="server" ContentPlaceHolderID="PageHead">
<style type="text/css">
.moduel_wrap1_profile table{border-collapse: collapse;width: 100%;}
.moduel_wrap1_profile table, .moduel_wrap1_profile table tr, .moduel_wrap1_profile table tr td{border: 1px solid #ccc;padding: 5px;}
</style>
</asp:Content>
<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
    <div class="moduel_wrap moduel_width1">
        <h3 class="title">
            <span class="bg3">酒店详情</span></h3>
        <div class="moduel_wrap1 moduel_wrap1_profile">            
            <table cellspacing="1" cellpadding="1" border="1" width="550">
                <tbody>
                    <tr>
                        <td align="center" width="50px">
                            酒店名称
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblHotelName" runat="server" Text="暂无"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="50px">
                            星级
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblHotelStar" runat="server" Text="暂无"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="50px">
                            地址
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblHotelAddress" runat="server" Text="暂无"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="50px">
                            酒店图片
                        </td>
                        <td colspan="2" width="585px">
                            <%=photos %>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="50px">
                            酒店简介
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblHotelInfo" runat="server" Text="暂无"></asp:Label>
                        </td>
                    </tr>
                    <asp:Repeater runat="server" ID="rpt">
                        <ItemTemplate>
                            <tr>
                                <td align="center" rowspan="3" width="50px">
                                    房间
                                </td>
                                <td align="right" width="50px">
                                    房型
                                </td>
                                <td width="368">
                                    <%#Eval("Name").ToString()!=""?Eval("Name"):"暂无"%>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="50px">
                                    早餐
                                </td>
                                <td>
                                    <%#Eval("IsBreakfast").ToString().ToUpper()=="TRUE"?"有":"无" %>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" align="right" width="50px">
                                    价格
                                </td>
                                <td>
                                    <font style="color: Red">￥<%# Convert.ToDecimal(Eval("SellingPrice")).ToString("f2") %></font>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div style="width: 100%; text-align: center">
                <a href="javascript:history.go(-1);" style="font-size: 14px; color: #004784">返 回</a>
            </div>
        </div>
    </div>
</asp:Content>
