<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bbllogin.aspx.cs" Inherits="Web.GroupEnd.bbllogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>登录_52我爱中华</title>
    <link href="/css/yangshi.css" rel="stylesheet" type="text/css" />
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <script src="/js/jquery.js" type="text/javascript"></script>
    <script src="/js/back.js" type="text/javascript"></script>
    <script src="/js/slogin.js" type="text/javascript"></script>
    <script src="/js/jquery.boxy.js" type="text/javascript"></script>
</head>
<body background="/images/index_01.jpg" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
<form>
<table width="960" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="24" align="right" class="bai"><a href="#">设为首页</a> | <a href="#">收藏本站</a> |<a href="#"> 联系我们</a> </td>
  </tr>
</table>
<table width="960" border="0" align="center" cellpadding="0" cellspacing="0" style=" border-bottom:3px solid #4A7F02 ">
  <tr>
    <td height="90" colspan="2" valign="top"><img src="/images/index_04.gif" width="227" height="77" /></td>
    <td width="166" rowspan="5" align="right" valign="middle">
        <marquee direction="down" width="142px" height="490px" onmousemove="stop()" onmouseout="start()" scrollamount="4">
        <table width="141" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <img src="/images/index_04.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_06.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_04.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_06.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_04.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_06.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_04.jpg" width="141" height="82" />
                </td>
            </tr>
            <tr>
                <td>
                    <img src="/images/index_06.jpg" width="141" height="82" />
                </td>
            </tr>
        </table>
        </marquee>
    </td>
  </tr>
  <tr>
    <td height="60" colspan="2" valign="top"><table border="0" align="center" cellpadding="0" cellspacing="0" class="nav">
      <tr>
        <td><a href="#">首 页</a></td>
        <td><a href="#">机票查询</a></td>
        <td><a href="#">火车票查询</a></td>
        <td><a href="#">酒店查询</a></td>
        <td><a href="#">同业资讯 </a></td>
        <td><a href="#">电子杂志</a></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td colspan="2"><img src="/images/index_12.jpg" width="794" height="290" /></td>
  </tr>
  <tr>
    <td width="398"><img src="/images/index_11.jpg" /></td>
    <td width="396" background="/images/index_12.gif"><table width="335" border="0" cellspacing="0" cellpadding="0" style="margin-left:8px;">
        <tr>
            <td colspan="3" align="center">
                <span id="spanMsg" class="errmsg" style="color:Red"></span>
            </td>
        </tr>
      <tr>
        <td width="56" height="24" align="right"><font color="#666666">用户名：</font></td>
        <td width="142" height="24"><input tabindex="1" type="text" name="u" id="u" style="width:120px;" /></td>
        <td height="24" colspan="2">&nbsp;</td>
      </tr>
      <tr>
        <td height="24" align="right"><font color="#666666">密&nbsp;&nbsp;码：</font></td>
        <td height="24"><input type="password" tabindex="5" type="text" name="p" id="p" style="width:120px;" /></td>
        <td height="24" colspan="2">&nbsp;</td>
      </tr>
      <tr>
        <td height="24" align="right"><font color="#666666">验证码：</font></td>
        <td height="24"><input onfocus="this.select();" tabindex="10" style="width: 50px;" type="text" name="vc"
                                id="vc" />
          <img style="cursor: pointer;" title="点击更换验证码" onclick="this.src='/ashx/ValidateCode.ashx?ValidateCodeName=bbl&id='+Math.random();return false;"
                                align="absmiddle" width="60" height="20" id="imgCheckCode" src="/ashx/ValidateCode.ashx?ValidateCodeName=bbl&t=<%=DateTime.Now.ToString("HHmmssffff") %>" /></td>
        <td width="68" height="24"> <a id="linkLogin" tabindex="15" href="javascript:;"><img src="/images/denglui.gif" width="62" height="22" /></a></td>
        <td width="69" height="24">&nbsp;</td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td colspan="2">
        <table width="792" cellspacing="0" cellpadding="0" border="0" align="center" style="margin: 25px auto;">
      <tbody><tr>
        <td width="132" align="center"><a href="http://www.csair.com?WT.mc_id=afl-exl-wazhlxyq-shouye" target="_blank"><img width="61" height="62" border="0" src="/images/logo7.jpg"></a></td>
        <td width="132" align="center"><a href="#"><img width="61" height="62" border="0" src="/images/logo2.gif"></a></td>
        <td width="132" align="center"><a href="#"><img width="61" height="62" border="0" src="/images/logo4.gif"></a></td>
        <td width="132" align="center"><a href="#"><img width="61" height="62" border="0" src="/images/logo3.gif"></a></td>
        <td width="132" align="center"><a href="#"><img width="61" height="62" border="0" src="/images/logo5.gif"></a></td>
        <td width="132" align="center"><a href="#"><img width="61" height="62" border="0" src="/images/logo1.gif"></a></td>
      </tr>
      <tr>
        <td width="132" height="28" align="center" class="huang"><a href="http://www.csair.com?WT.mc_id=afl-exl-wazhlxyq-shouye" target="_blank">南方航空公司</a></td>
        <td width="132" height="35" align="center" class="huang"><a href="#">中国东方航空公司 </a></td>
        <td width="132" height="35" align="center" class="huang"><a href="#">海南航空公司 </a></td>
        <td width="132" height="35" align="center" class="huang"><a href="#">中国国际航空 </a></td>
        <td width="132" height="35" align="center" class="huang"><a href="#">深圳航空公司</a></td>
        <td width="132" height="35" align="center" class="huang"><a href="#">厦门航空公司</a></td>
      </tr>
    </tbody></table>
    </td>
  </tr>
</table>
<table width="960" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td height="50" align="center" class="bai">版权所有：巴比来 技术支持：杭州易诺科技 </td>
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
</form>
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
                        }
                    }
                    var win = window.open(s,"_blank","width="+(window.screen.width-10)+"px,height="+(window.screen.height-80)+"px, toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,resizable=no,status=no,left=0,top=0");
                    
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
                           if (win.outerWidth === 0) {
                                win.close();
                                popBlockedTip();
                           }
                        };
                    }else{
                        setTimeout(function(){
                            window.opener=null;
                            window.open("","_self");
                            window.close();
                        },10);
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
</body>
</html>
