<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WhLoginControl.ascx.cs"
    Inherits="Web.UserControl.wh.WhLoginControl" %>
<div class="moduel_wrap moduel_width" style="clear: both;">
    <h3 class="title"><span class="bg1">用户登录</span></h3>
    <!--未登录面板 开始-->        
    <div class="moduel_wrap1" runat="server" id="divUnLogin">
        <span id="spanMsg" class="errmsg" style="color: Red"></span>
        <div class="loginForm" style="margin: 0; padding: 0;">
            <label for="">
                用户名：</label><input type="text" tabindex="1" id="u" name="" value="" size="28" /><br />
            <label for="">
                密&nbsp;&nbsp;&nbsp;码：</label><input tabindex="5" id="p" type="password" name="" value="" size="28" /><br />
            <label for="">
                验证码：</label><input onfocus="this.select();" tabindex="10" style="width: 50px;" type="text" name="vc" id="vc" value="" size="10" />
            <span>
                <img style="cursor: pointer; vertical-align: middle;" title="点击更换验证码" onclick="this.src='/ashx/ValidateCode.ashx?ValidateCodeName=bbl&id='+Math.random();return false;" align="absmiddle" width="60" height="20" id="imgCheckCode" src="/ashx/ValidateCode.ashx?ValidateCodeName=bbl&t=<%=DateTime.Now.ToString("HHmmssffff") %>" /></span><br />
            <input type="submit" value="登录" class="submit" id="linkLogin" tabindex="15" /><br />
            <a class="link2btn" title="" tabindex="15" href="<%=RegUri %>" style="margin-left: 55px !important; margin-left: 27px;">组团(零售)社注册</a>
            <div class="clearBoth">
            </div>
        </div>
    </div>
    <!--未登录面板 结束-->
    <!--已登录面板 开始-->
    <div class="moduel_wrap1" runat="server" id="divLogin">
        <div style="line-height: 24px;">
            欢迎您！<a href="/groupend/default.aspx">进入后台<br />
                <a href="/logout.aspx">退出登录</a>
        </div>
    </div>
    <!--已登录面板 结束-->
</div>

<script src="/js/slogin.js" type="text/javascript"></script>

<script type="text/javascript">
    function getCheckCode() {
        var c = document.cookie, ckcode = "", tenName = "";
        for (var i = 0; i < c.split(";").length; i++) {
            tenName = c.split(";")[i].split("=")[0];
            ckcode = c.split(";")[i].split("=")[1];
            if ($.trim(tenName) == "bbl") {
                break;
            } else {
                ckcode = "";
            }
        }
        return $.trim(ckcode);
    };
    function login() {
        var u = $.trim($("#u").val()), p = $.trim($("#p").val()), ckcode = $.trim($("#vc").val());
        if (u == "") {
            $("#spanMsg").html("请输入用户名");
            return;
        }
        if (p == "") {
            $("#spanMsg").html("请输入密码");
            return;
        }
        if (ckcode != getCheckCode()) {
            $("#spanMsg").html("请输入正确的验证码");
            return;
        }
        //显示登录状态
        $("#spanMsg").html("正在登录中...");
        //防止重复登陆
        $("#linkLogin").unbind().css("cursor", "default");
        blogin5($("form").get(0), function(h) {
            $("#spanMsg").html("登录成功，正进入系统...");
            var s = '<%=Request.QueryString["returnurl"] %>';
            if (s == "") {
                if (h == "1") {
                    s = "/GroupEnd/Default.aspx";
                } else if (h == "2") {
                    s = "/Default.aspx";
                }
            }

            $("#<%=divUnLogin.ClientID %>").hide();
            $("#<%=divLogin.ClientID %>").show();

            var win = window.open(s, "_blank", "width=" + (window.screen.width - 10) + "px,height=" + (window.screen.height - 80) + "px, toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,resizable=no,status=no,left=0,top=0");

            var popBlockedTip = function() {
                $("#linkManage").attr("href", s);
                var h = $("#trMsg").html();
                new Boxy(h, { title: "登录提示", modal: true, afterHide: function() {
                    window.location.reload(true);
                }
                });
            }

            if (win == null) {
                popBlockedTip();
            } else if (win && jQuery.browser.safari) {
                win.onload = function() {
                    if (win.outerWidth === 0) {
                        win.close();
                        popBlockedTip();
                    }
                };
            } else {
                setTimeout(function() {
                    window.opener = null;
                    window.open("", "_self");
                    window.close();
                }, 0);
            }


        }, function(m) {
            $("#spanMsg").html(m);
            //重新绑定登录事件
            $("#linkLogin").click(function() {
                login(); return false;
            }).css("cursor", "pointer");
        });
    }
    $(function() {
        $("#u").focus();
        $("#linkLogin").click(function() {
            login();
            return false;
        });
        $("#u,#p,#vc").keypress(function(e) {
            if (e.keyCode == 13) {
                login();
                return false;
            }
        });

    });
</script>

