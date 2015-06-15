<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LotsDengji.aspx.cs" Inherits="Web.caiwuguanli.LotsDengji" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="/css/sytle.css" />

    <script src="/js/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="/js/datepicker/WdatePicker.js"></script>

</head>
<body>
    <form runat="server" id="form1">
    <table width="900" cellspacing="1" cellpadding="0" border="0" align="center" style="margin: 10px;">
        <tbody>
            <tr class="odd">
                <th width="110" height="30" bgcolor="#bddcf4" align="center">
                    付款时间
                </th>
                <th width="125" bgcolor="#bddcf4" align="center">
                    支付方式
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    付款人
                </th>
                <th width="110" bgcolor="#bddcf4" align="center">
                    备注
                </th>
                <th width="80" bgcolor="#bddcf4" align="center">
                    操作
                </th>
            </tr>
            <tr class="even">
                <td height="30" align="center">
                    <font class="xinghao">*</font><asp:TextBox ID="txtPayDate" name="txtPayDate" runat="server"
                        onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'});" Width="80%" errmsg="付款时间不能为空！"></asp:TextBox>
                </td>
                <td align="center">
                    <font class="xinghao">*</font><asp:DropDownList ID="ddlPayType" name="ddlPayType"
                        runat="server" Width="80%" DataTextField="Text" DataValueField="Value">
                    </asp:DropDownList>
                </td>
                <td align="center">
                    <font class="xinghao">*</font><asp:TextBox ID="txtStaffName" runat="server" Width="80%"
                        errmsg="付款人不能为空！"></asp:TextBox>
                </td>
                <td align="center">
                    <textarea rows="2" cols="20" id="t_desc" name="t_desc" width="80%" runat="server"></textarea>
                </td>
                <td align="center">
                    <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click" OnClientClick="return CheckingSave()">保存</asp:LinkButton>
                </td>
            </tr>
        </tbody>
    </table>
    <table width="320" cellspacing="0" cellpadding="0" border="0" align="center">
        <tbody>
            <tr>
                <td height="40" align="center">
                </td>
                <td height="40" align="center" class="tjbtn02">
                    <a onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeid"] %>').hide()"
                        id="linkCancel" href="javascript:;">关闭</a>
                </td>
            </tr>
        </tbody>
    </table>

    <script type="text/javascript">
        function CheckingSave() {
            var msg = TestNoNULL("id", "txtPayDate,txtStaffName");
            if (msg.length > 0) {
                alert(msg);
                return false;
            }
            return true;
        }
        /*文本框非空验证*/
        //key指定属性，value指定属性名称
        function TestNoNULL(key, names) {
            var msg = "";
            if (names.length > 0) {
                var values = names.split(",");
                for (var i = 0; i < values.length; i++) {
                    var obj = $("input[" + key + "='" + values[i] + "']");
                    if ($.trim(obj.val()).length <= 0) {
                        msg += obj.attr("errmsg") + " \n";
                    }
                }
            }
            return msg;
        }
    </script>

    </form>
</body>
</html>
