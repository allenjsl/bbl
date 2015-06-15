<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="self.aspx.cs" Inherits="Web.Webmaster.self" MasterPageFile="~/Webmaster/mpage.Master" %>

<%@ MasterType VirtualPath="~/Webmaster/mpage.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="Scripts" ID="ScriptsContent">
    <style type="text/css">
        .trspace{height:10px;  font-size:0px;}
        .note {color:#999; margin-left:5px; }
        .required {color:#ff0000}
        .unrequired{color:#fff}
    </style>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageTitle" ID="TitleContent">
    我的信息管理
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageContent" ID="MainContent">
    <table cellpadding="2" cellspacing="1" style="font-size: 12px; width: 100%;">
        <tr>
            <td>
                登录账号：<input type="text" id="txtUsername" name="txtUsername" class="input_text" maxlength="72" style="width: 400px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                登录密码：<input type="password" id="txtPassword" name="txtPassword" class="input_text" maxlength="72" style="width: 400px" runat="server" /><span class="note">密码为空时不做修改</span>
            </td>
        </tr>
        <tr class="trspace">
            <td></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnUpdate" runat="server" Text="修改登录密码" OnClick="btnUpdate_Click" />
            </td>
        </tr>
        <tr class="trspace">
            <td>
            </td>
        </tr>
        <tr>
            <td>
                <input type="text" id="txtPlaintext" name="txtPlaintext" class="input_text" maxlength="72" style="width: 200px" runat="server" />
                <asp:Button ID="btnMD5Encrypt" runat="server" Text="查看MD5值" OnClick="btnMD5Encrypt_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="PageRemark" ID="RemarkContent">
</asp:Content>
