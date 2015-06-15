<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="slogin.aspx.cs" Inherits="Web.slogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>登录</title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/slogin.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="300" border="0" align="center" cellpadding="0" cellspacing="1" style="margin: 10px;">
        <tr>
            <th width="100" height="35" align="center">
                用户名：
            </th>
            <td width="195">
                <input tabindex="1" type="text" name="u" id="u" class="widowkuangtext" />
            </td>
        </tr>
        <tr>
            <th width="100" height="35" align="center">
                密&nbsp; 码：
            </th>
            <td width="195">
                <input type="password" tabindex="5" type="text" name="p" id="p" class="widowkuangtext" />
            </td>
        </tr>
         <tr>
            <th width="100" height="35" align="center">
               &nbsp;
            </th>
            <td align="center" class="tjbtn02">
                <a href="javascript:;" id="linkLogin" tabindex="15">登录</a>
            </td>
        </tr>
        <tr>
            <th width="100" height="35" align="center">
               &nbsp;
            </th>
            <td align="center" class="tjbtn02">
                 &nbsp;<span id="spanMsg" class="errmsg"></span>
            </td>
        </tr>
    </table>
    </div>
    <input type="hidden" name="vc" id="vc" />
<script type="text/javascript">
        function login(){
            var u = $.trim($("#u").val()),p =$.trim($("#p").val());
            if(u==""){
                $("#spanMsg").html("请输入用户名");
                return;
            }
            if(p==""){
                $("#spanMsg").html("请输入密码");
                return;
            }
            //显示登录状态
            $("#spanMsg").html("正在登录中...");
            //防止重复登陆
            $("#linkLogin").unbind().css("cursor","default");
            blogin5($("form").get(0),function(){
                    
                    var s = "<%=Request.QueryString["returnurl"] %>";
                    if(s==""){
                        $("#spanMsg").html("登录成功");
                    }else{
                        $("#spanMsg").html("登录成功，正在跳转...");
                        window.location.href=s;
                    }
                },function(m){
                    $("#spanMsg").html(m);
                    //重新绑定登录事件
                    $("#linkLogin").click(function(){
                        login();return false;
                    }).css("cursor","pointer");
                });
        }
        $(function(){
            $("#u").focus();
            $("#linkLogin").click(function(){
                login();
                return false;
            });
            $("#u,#p,#vc").keypress(function(e){
                if (e.keyCode == 13) {
                   login();
                   return false;
                }
            });
        });
        </script>
    </form>
</body>
</html>
