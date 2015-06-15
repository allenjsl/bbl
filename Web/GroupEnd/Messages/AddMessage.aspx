<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMessage.aspx.cs" Inherits="Web.GroupEnd.Messages.AddMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
                        <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
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
                        <asp:TextBox ID="txtMsg" TextMode="MultiLine" runat="server" Height="120px" Width="500px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" align="center" colspan="2">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="btnSave" runat="server" OnClientClick="return CheckForm()" OnClick="btnSave_Click">保存</asp:LinkButton>
                                    </td>
                                    <td align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                                            id="linkCancel" href="javascript:void(0);">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <script type="text/javascript">
        function CheckForm() {
            var msg = $.trim($("#<%=txtMsg.ClientID %>").val());
            var msgtitle = $.trim($("#<%=txtTitle.ClientID %>").val());
            if (msg == "") {
                alert("请输入回复信息!")
                return false;
            }
            if (msgtitle == "") {
                alert("请输入标题!");
                return false;
            }
            return true;
        }
    </script>
    </form>
</body>
</html>