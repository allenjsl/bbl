<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updateStatus.aspx.cs" Inherits="Web.jipiao.Traffic.updateStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../js/jquery.js"></script>

    <script type="text/javascript" src="../../js/ValiDatorForm.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="300" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0 auto;">
        <tr class="odd">
            <th width="30%" height="30" align="right">
                请选择：
            </th>
            <td width="70%" bgcolor="#E3F1FC"> 
                <label>
                    <input name="radStatus" type="radio" value="0" <%=ticketStatus=="0"?"checked='checked'":"" %> />
                    正常</label>
                <label>
                    <input type="radio" name="radStatus" value="1" <%=ticketStatus=="1"?"checked='checked'":"" %> />
                    停售</label>
            </td>
        </tr>
        <tr class="even">
            <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                <table width="100" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:" class="Save">保存</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        $(function() {
            $(".Save").click(function() {
                var status = $("#<%=form1.ClientID %>").serialize();
                var Id = '<%=EyouSoft.Common.Utils.GetQueryStringValue("Id") %>';
                var tfId = '<%=EyouSoft.Common.Utils.GetQueryStringValue("tfId") %>';
                $.newAjax({
                    url: '/jipiao/Traffic/updateStatus.aspx?type=update&Id=' + Id + "&tfId=" + tfId,
                    type: 'POST',
                    cache: false,
                    data: status,
                    success: function(ret) {
                        var obj = eval('(' + ret + ')');
                        alert(obj.msg);
                        parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                        parent.window.location.reload();
                    },
                    error: function() {
                        alert("服务器繁忙，请稍后在试!");
                    }
                });
                return false;
            });
        });
    </script>

</body>
</html>
