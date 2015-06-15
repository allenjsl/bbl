<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdatePwd.aspx.cs" Inherits="Web.GroupEnd.LineList.UpdatePwd" Title="修改密码" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>修改密码</title>
     <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="300" border="0" align="center" cellpadding="0" cellspacing="0" style="margin:10px;">
          <tr class="odd">
            <th width="109" height="35" align="center" valign="middle">原密码：</th>
            <td width="191" align="left"><input type="text" name="oldpas" id="TxtPwdOld" class="companyInfo02" /></td>
          </tr>
          <tr class="even">
            <th height="35" align="center" valign="middle">新密码：</th>
            <td align="left"><input type="text" name="newpas" id="TxtPwdNew" class="companyInfo02" /></td>
          </tr>
          <tr class="odd">
            <th height="35" align="center" valign="middle">确认密码：</th>
            <td align="left"><input type="text" name="newpas2" id="TxtPwdSubmit" class="companyInfo02" /></td>
          </tr>
        </table>
        <table width="320" border="0" align="center" cellpadding="0" cellspacing="0">
          <tr>
            <td height="40" align="center"></td>
            <td align="center" class="tjbtn02"><a href="javascript:;" id="BtnSubmit">确认</a></td>
            <td height="40" align="center" class="tjbtn02">
              <a href="javascript:;" id="linkCancel" onclick="parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide()">关闭</a></td>
          </tr>
      </table>
    </div>
    <script type="text/javascript">
        var UpdatePwd = {
            OnSubmit: function() {
                if($("#TxtPwdNew").val().length<1){
                    alert("新密码不能为空！");
                    return false;
                }
                if($("#TxtPwdNew").val()!=$("#TxtPwdSubmit").val()){
                    alert("两次密码输入不同！");
                    return false;
                }
                $.newAjax({                    
                    type: "POST",
                    url: "/GroupEnd/LineList/UpdatePwd.aspx",
                    cache: false,
                    data: $("#BtnSubmit").closest("form").serialize(),
                    dataType: 'json',
                    success: function(msg) {
                        if(msg.isSuccess=="True"){
                            alert(msg.errMsg);
                            parent.Boxy.getIframeDialog('<%=Request.QueryString["iframeId"] %>').hide();
                            return false;
                        }else{
                            alert(msg.errMsg);
                            return false;
                        }
                    },
                    error: function() {
                        alert("服务器繁忙!请稍候再进行此操作!");
                        return false;
                    }
                });
                return false;
            }
        };
        $(document).ready(function() {
            $("#BtnSubmit").click(function() {
                UpdatePwd.OnSubmit();
                return false;
            });
        });
    </script>
    </form>
</body>
</html>
