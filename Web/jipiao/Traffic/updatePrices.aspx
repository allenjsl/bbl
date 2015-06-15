<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="updatePrices.aspx.cs" Inherits="Web.jipiao.Traffic.updatePrices" %>

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
            <th width="29%" height="30" align="right">
                成本：
            </th>
            <td width="71%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" id="txtTicketPrices" size="10" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th width="29%" height="30" align="right">
                总票数：
            </th>
            <td width="71%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" id="txtTicketNums" size="10" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <th width="29%" height="30" align="right">
                状态：
            </th>
            <td width="71%" bgcolor="#E3F1FC">
                <label>
                    <input type="radio" name="radStatus" value="0" <%=status.ToString() == "0" ? "checked='checked'" : ""%> />
                    正常</label>
                <label>
                    <input type="radio" name="radStatus" value="1" <%=status.ToString() == "0" ? "" : "checked='checked'"%> />
                    停售</label>
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                <table width="100" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:" class="Save"><%=EyouSoft.Common.Utils.GetQueryStringValue("type")=="add"?"添加":"修改" %></a>
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
                var data = $("#<%=form1.ClientID %>").serialize();
                var Id = '<%=EyouSoft.Common.Utils.GetQueryStringValue("Id") %>';
                var date = '<%=EyouSoft.Common.Utils.GetQueryStringValue("date") %>';
                var tfId = '<%=EyouSoft.Common.Utils.GetQueryStringValue("tfId") %>';
                var type = '<%=EyouSoft.Common.Utils.GetQueryStringValue("type") %>';
                $.newAjax({
                    url: '/jipiao/Traffic/updatePrices.aspx?action=Save&pricesId=' + Id + "&date=" + date + "&tfId=" + tfId + "&type=" + type,
                    type: 'POST',
                    cache: false,
                    data: data,
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
