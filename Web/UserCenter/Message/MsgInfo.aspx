<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MsgInfo.aspx.cs" Inherits="Web.UserCenter.Message.MsgInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/datepicker/WdatePicker.js" type="text/javascript"></script>
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
                        <asp:TextBox ID="txtReplyMsg" runat="server" Height="120px" TextMode="MultiLine" 
                            Width="500px"></asp:TextBox>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="30" align="right">
                        <b>回复人：</b>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtReplyMan" runat="server" ></asp:TextBox>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="right">
                        <b>回复时间：</b></td>
                    <td align="left">
                        <asp:TextBox ID="txtReplyDate" runat="server" onfocus="WdatePicker()"></asp:TextBox></td>
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
            var msg = $.trim($("#<%=txtReplyMsg.ClientID %>").val());
            var msgMan = $.trim($("#<%=txtReplyMan.ClientID %>").val());
            var msgDate = $.trim($("#<%=txtReplyDate.ClientID %>").val());

            if (msg == "") {
                alert("请输入回复信息!")
                return false;
            }
            if (msgMan == "") {
                alert("请输入回复人名!");
                return false;
            }
            if (msgDate == "") {
                alert("请输入回复时间");
                return false;
            }
               
            return true;
        }
    </script>
    </form>
</body>
</html>
