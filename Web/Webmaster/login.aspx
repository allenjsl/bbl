<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Web.Webmaster.login" %>

<!DOCTYPE html >
<html>
<head>
    <title>巴比来项目管理系统WEBMASTER</title>
    <style type="text/css">	
	body {font-size: 14px; font-family: Verdana; margin-top:10px; margin-left:20px; margin-right:20px;}
    a:link {color: #31659C;text-decoration: none;}
    a:visited {color: #31659C;text-decoration: none;}
    a:hover {color: #CE6500;text-decoration: none;}
    form {margin:0px;padding:0px;}
    .input_text{border:1px solid #003c74;font-size: 11pt;width: 180px;cursor: text;height: 18px;}
	</style>

    <script type="text/javascript" src="/js/jquery.1.5.2.min.js"></script>

    <script type="text/javascript">
        var _p = parent;

        try {
            var _iframe = _p.opiframe || _p.window.frames["opiframe"] || _p.document.getElementById("opiframe").contentWindow;
            _p.location.href = this.location;
        }
        catch (e) { }

        function WebForm_OnSubmit_Validate() {
            if ($.trim($("#t_u").val()) == "") {
                alert('Please enter your login information.');
                $("#t_u").focus();
                return false;
            }
            if ($.trim($("#t_p").val()) == "") {
                alert('Please enter a password.');
                $("#t_p").focus();
                return false;
            }
            return true;
        }

        $(document).ready(function() {
            $("#t_u").focus();
            $("#<%=btnLogin.ClientID %>").bind("click", function() { return WebForm_OnSubmit_Validate(); });
        });
    </script>
    <%--text:000000 MD5:670b14728ad9902aecba32e22fa4f6bd--%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            巴比来项目管理系统，开发者推荐使用<a href="http://www.firefox.com.cn/" target="_blank">firefox</a>或<a href="http://www.google.cn/chrome/intl/zh-CN/landing_chrome.html?hl=zh-CN&brand=CHMI" target="_blank">google</a>浏览器。
            <br />
            <br />
        </div>
        <div>
            <p>
                登录账号：<input id="t_u" class="input_text" name="t_u" size="20" type="text" />
            </p>
            <p>
                登录密码：<input id="t_p" class="input_text" name="t_p" size="20" type="password" />
            </p>
            <p>
                <asp:Button runat="server" ID="btnLogin" OnClick="btnLogin_Click" Text="login" />
            </p>
        </div>
        <div>
            <br />
            <br />
            <span style="font-family: Arial,Helvetica,sans-serif">Copyright &copy; 2008-2012 杭州易诺科技，All Rights Reserved.</span>
            <br />
            <br />
        </div>
    </div>
    </form>
</body>
</html>
