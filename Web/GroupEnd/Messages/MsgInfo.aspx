<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MsgInfo.aspx.cs" Inherits="Web.GroupEnd.Messages.MsgInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="800" style="margin-top: 10px;">
            <tbody>
                <tr class="odd">
                    <th height="30" align="right" width="3%">
                        <b>标题：</b>
                    </th>
                    <td align="left" width="10%">
                        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="right">
                        <b>留言时间：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblMsgDate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" align="right">
                        <b>留言人：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblMsgMan" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="right">
                        <b>留言内容：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" align="right">
                        <b>回复状态：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblState" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="right">
                        <b>回复内容：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblReplyMsg" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" align="right">
                        <b>回复人：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblReplyMan" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="right">
                        <b>回复时间：</b>
                    </td>
                    <td align="left">
                        <asp:Label ID="lblReplyDate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
     
    </form>
</body>
</html>
