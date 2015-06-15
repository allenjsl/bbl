<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trafficAdd.aspx.cs" Inherits="Web.jipiao.Traffic.trafficAdd" %>

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
    <table width="690" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 0 auto;">
        <tr class="odd">
            <th width="17%" height="30" align="right">
                <span class="fred">*</span>交通名称：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" id="txtTrfficName" size="40" runat="server" valid="required"
                    errmsg="请输入交通名称!" />
                <span id="errMsg_txtTrfficName" class="errmsg"></span>
            </td>
        </tr>
        <tr class="odd">
            <th width="17%" height="30" align="right">
                儿童价：
            </th>
            <td width="83%" bgcolor="#E3F1FC">
                <input type="text" class="xtinput" id="txtChildprices" size="40" runat="server" />
            </td>
        </tr>
        <tr class="odd">
            <td height="30" colspan="8" align="left" bgcolor="#E3F1FC">
                <table width="100" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="40" align="center" class="tjbtn02">
                            <a href="javascript:" id="save">保存</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script type="text/javascript">
        $(function() {
            //获取表单验证
            FV_onBlur.initValid($("#<%=form1.ClientID %>").get(0));

            //保存
            $("#save").click(function() {
                var vResult = ValiDatorForm.validator($("#<%=form1.ClientID %>").get(0), "span");
                var action = '<%=Request.QueryString["type"] %>';
                var ID = '<%=Request.QueryString["trfficID"] %>';
                if (vResult) {
                    $.ajax({
                        url: '/jipiao/Traffic/trafficAdd.aspx?type=' + action + '&action=Save&trfficID=' + ID,
                        type: 'POST',
                        cache: false,
                        data: $("#<%=form1.ClientID %>").serialize(),
                        success: function(ret) {
                            var obj = eval('(' + ret + ')');
                            if (obj.ret == "1") {
                                alert(obj.msg);
                                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                            }
                            else {
                                alert(obj.msg);
                                parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                            }
                            parent.location.reload();
                        },
                        error: function() {
                            alert("服务器繁忙，请稍后在试!");
                        }
                    });
                }
                return false;
            });
        })
    </script>

</body>
</html>
