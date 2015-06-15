<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Web.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录_<%=CompanyName%></title>
    <link href="/css/sytle.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        img
        {
            border: 0;
        }
    </style>

    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/back.js" type="text/javascript"></script>
    <script src="/js/slogin.js" type="text/javascript"></script>
    <script src="/js/jquery.boxy.js" type="text/javascript"></script>

</head>
<body style="background: #DEEEFD">

    <form id="form1" runat="server">
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" class="loginbox"
        style="background: url(/images/login05.jpg) repeat-x center top; margin: 0; padding: 0;">
        <tr class="logintop" style="<% if (BigLogo!=null && BigLogo.Trim() != ""){%>background:url(<%=BigLogo%>) no-repeat left center;<%}else{%>background: no-repeat left center;<%} %>">
            <td height="91" valign="bottom">
                <table width="27%" border="0" align="right" cellpadding="0" cellspacing="0" class="datetimesbox">
                    <tr>
                        <td height="30">
                            <img src="/images/dateicon.gif" />现在时间：<span id="timeDiv"><%=DateTime.Now.ToString("yyyy年M月d日 dddd HH:mm:ss")   %></span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <img src="/images/login02.jpg" width="998" height="137" />
            </td>
        </tr>
        <tr>
            <td align="center" valign="top" class="textbox" style="background: url(/images/login03.jpg) no-repeat center center;
                height: 186px;">
                <table width="400" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="right" colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="132" height="35" align="right">
                            用户名：
                        </td>
                        <td width="9" align="center">
                            &nbsp;
                        </td>
                        <td width="165" align="left">
                            <input tabindex="1" type="text" name="u" id="u" value="" />
                        </td>
                        <td width="94" rowspan="3" align="left" valign="middle">
                            <a id="linkLogin" tabindex="15" href="javascript:;">
                                <img src="/images/login04.jpg" width="68" height="69" /></a>
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right">
                            密 码：
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="left">
                            <input type="password" tabindex="5" type="text" name="p" id="p" value="" />
                        </td>
                    </tr>
                    <tr>
                        <td height="35" align="right">
                            验证码：
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                        <td align="left">
                            <input onfocus="this.select();" tabindex="10" style="width: 50px;" type="text" name="vc" value=""
                                id="vc" />
                            <img style="cursor: pointer;" title="点击更换验证码" onclick="this.src='/ashx/ValidateCode.ashx?ValidateCodeName=bbl&id='+Math.random();return false;"
                                align="absmiddle" width="60" height="20" id="imgCheckCode" src="/ashx/ValidateCode.ashx?ValidateCodeName=bbl&t=<%=DateTime.Now.ToString("HHmmssffff") %>" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <span id="spanMsg" class="errmsg"></span>
                        </td>
                    </tr>
                </table>
                <div id="trMsg" style="color:Red;display:none;">
                <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin:10px;">
                    <tr>
                        <td height="60">
                            <span style="font-size: 14px; font-weight: bold; color: #FF0000; text-indent: 24px;">
                                尊敬的客户： 您已经登录成功 若系统后台没有自动弹出，请点这里进入系统后台！ <a id="linkManage" href="#" style="text-decoration: underline"
                                    target="_self">进入系统后台</a></span>
                        </td>
                    </tr>
</table>
                </div>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="margin-top: 20px;">
        <tr>
            <td height="35" align="center">
                版权所有：<%=CompanyName%> 技术支持：杭州易诺科技
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        <% DateTime tmpDate = DateTime.Now;%>
        serverDate = new Date(
        <%=tmpDate.Year %>,
        <%=tmpDate.Month-1 %>,
        <%=tmpDate.Day %>,
        <%=tmpDate.ToString("HH") %>,
        <%=tmpDate.Minute %>,
        <%=tmpDate.Second %>);
        setInterval('getNowDate()',1000);
    </script>

    <script type="text/javascript">
        function getCheckCode(){
            var c = document.cookie,ckcode="",tenName="";
            for (var i = 0; i < c.split(";").length; i++) {
                tenName = c.split(";")[i].split("=")[0];
                ckcode = c.split(";")[i].split("=")[1];
                if ($.trim(tenName) == "bbl") {
                    break;
                }else{
                    ckcode="";
                }
            }
            return $.trim(ckcode);
        };
        function login(){
            var u = $.trim($("#u").val()),p =$.trim($("#p").val()),ckcode=$.trim($("#vc").val());
            if(u==""){
                $("#spanMsg").html("请输入用户名");
                return;
            }
            if(p==""){
                $("#spanMsg").html("请输入密码");
                return;
            }
            if(ckcode!=getCheckCode()){
                $("#spanMsg").html("请输入正确的验证码");
                return;
            }
            //显示登录状态
            $("#spanMsg").html("正在登录中...");
            //防止重复登陆
            $("#linkLogin").unbind().css("cursor","default");
            blogin5($("form").get(0),function(h){
                    $("#spanMsg").html("登录成功，正进入系统...");
                    var s = "<%=Request.QueryString["returnurl"] %>";
                    if(s==""){
                        if(h=="1"){
                            s="/GroupEnd/Default.aspx";
                        }else if(h=="2"){
                            s="/Default.aspx";
                        }else if(h=="3"){
                            s="/UserCenter/DjArrangeMengs/TravelAgencyList.aspx";
                        }
                    }
                    
                    
                    var win = window.open(s,"_blank","width="+(window.screen.width-10)+"px,height="+(window.screen.height-80)+"px, toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,resizable=no,status=no,left=1,top=0");
                    
                    
                    var popBlockedTip=function(){
                        $("#linkManage").attr("href",s);
                        var h=$("#trMsg").html();
                        new Boxy(h,{title:"登录提示",modal:true,afterHide:function(){
                            window.location.reload(true);
                        }});
                    }
                    
                    
                    if(win==null){
                        popBlockedTip();
                    } else if(win && jQuery.browser.safari){
                        win.onload = function() {
                           if (win.screenX === 0) {
                                win.close();
                                popBlockedTip();
                           }
                        };
                    }else{
                        setTimeout(function(){
                            window.opener=null;
                            window.open("","_self");
                            window.close();
                        },0);
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
