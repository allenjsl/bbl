<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiPiaoZhengCeMingXi.aspx.cs" Inherits="Web.Shop.T1.JiPiaoZhengCeMingXi" MasterPageFile="~/Shop/T1/Default.Master" Title="机票政策明细" %>
<%--机票政策明细--%>
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
            <span class="bg3">机票政策</span></h3>
        <div class="moduel_wrap1 moduel_wrap1_profile">
            <table>
                <tr>
                    <th align="center">
                        <asp:Label ID="lblTitle" runat="server" Style="width: 630px; float: left; word-wrap: break-word;" Font-Size="Large"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <td align="center" style="width: 630px">
                        <asp:HyperLink ID="linkFilePath" runat="server" Visible="false" Target="_blank">下载附件</asp:HyperLink>
                        <hr style="width: 630px;" />
                    </td>
                </tr>
                <tr>
                    <td style="text-indent: 25px;">
                        <asp:Label ID="lblContent" runat="server" Style="width: 630px; float: left; word-wrap: break-word;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
