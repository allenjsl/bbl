<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tuituan.aspx.cs" Inherits="Web.sales.Tuituan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>订单退票操作</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script src="/js/ValiDatorForm.js" type="text/javascript"></script>

    <style type="text/css">
        .errmsg
        {
            color: red;
            font-size: 12px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellspacing="1" cellpadding="0" border="0" align="center" width="400" style="margin: 10px;">
            <tbody>
                <tr class="odd">
                    <td height="30" align="center" width="100">
                        团号：
                    </td>
                    <td align="left" width="398">
                        <asp:Label ID="lblTuanHao" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td height="30" align="center">
                        <span class="errmsg">*</span>金额：
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtMoney" runat="server" class="searchinput" name="txtMoney" errmsg="*请输入金额|*请输入有效金额！"
                            valid="required|isMoney" MaxLength="8"></asp:TextBox>
                        元   <span id="errMsg_<%=this.txtMoney.ClientID %>" class="errmsg"></span>
                    </td>
                </tr>
                <tr class="odd">
                    <td height="40" align="center">
                        原因：
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtReson" runat="server" TextMode="MultiLine" class="textareastyle02"
                            name="txtReson"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td height="40" align="center" colspan="2">
                        <table cellspacing="0" cellpadding="0" border="0" width="320">
                            <tbody>
                                <tr>
                                    <td height="40" align="center">
                                    </td>
                                    <td height="40" align="center" class="tjbtn02">
                                        <asp:LinkButton ID="lkBtnSave" runat="server" OnClick="lkBtnSave_Click">保存</asp:LinkButton>
                                    </td>
                                    <td height="40" align="center" class="tjbtn02">
                                        <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"]%>').hide()"
                                            id="linkCancel" href="javascript:;">关闭</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>

    <script type="text/javascript">
    $(function() {
        $("#<%=this.lkBtnSave.ClientID%>").click(function() {
            var form = $(this).closest("form").get(0);
            //点击按纽触发执行的验证函数
            return ValiDatorForm.validator(form, "span");
        });
        //初始化表单元素失去焦点时的行为，当需验证的表单元素失去焦点时，验证其有效性。
        FV_onBlur.initValid($("#<%=this.lkBtnSave.ClientID%>").closest("form").get(0));
    })
    </script>

</body>
</html>
