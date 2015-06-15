<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserReg.aspx.cs" Inherits="Web.GroupEnd.wh.UserReg" %>

<%@ Register Assembly="ControlLibrary" Namespace="Adpost.Common.ExporPage" TagPrefix="cc3" %>
<%@ Register Src="../../UserControl/wh/WhHeadControl.ascx" TagName="WhHeadControl"
    TagPrefix="uc1" %>
<%@ Register Src="../../UserControl/wh/WhBottomControl.ascx" TagName="WhBottomControl"
    TagPrefix="uc2" %>
<%@ Register Src="../../UserControl/wh/WhLoginControl.ascx" TagName="WhLoginControl"
    TagPrefix="uc3" %>
<%@ Register Src="../../UserControl/wh/WhLineControl.ascx" TagName="WhLineControl"
    TagPrefix="uc4" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<head id="Head1" runat="server">
    <title>
        <%=EyouSoft.Common.Utils.GetTitleByCompany("用户注册",this.Page) %></title>
    <link href="/css/boxy.css" rel="stylesheet" type="text/css" />
    <link href="/css/common.css" rel="stylesheet" type="text/css" />
    <link href="/css/style.css" rel="stylesheet" type="text/css" />
    <link href="/css/tipsy.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #loginForm input, .content_right input
        {
            border: 1px solid #A2C8D2;
            height: 20px;
            line-height: 170%;
            margin-top: 5px;
            outline: medium none;
            padding: 0 0 0 2px;
        }
        body, input, select, button, textarea
        {
            font-family: Tahoma,Geneva,sans-serif;
            font-size: 12px;
        }
        .moduel_wrap1_profile table
        {
            border-collapse: collapse;
            width: 100%;
        }
        .moduel_wrap1_profile table, .moduel_wrap1_profile table tr, .moduel_wrap1_profile table tr td
        {
            border: 0px solid #ccc;
            padding: 5px;
        }
    </style>

    <script src="/js/jquery.js" type="text/javascript"></script>

</head>
<body id="Body1" runat="server">
    <form id="Form1" runat="server">
    <div id="page">
        <uc1:WhHeadControl ID="WhHeadControl1" runat="server" />
        <!--mainContentStart-->
        <div class="content">
            <div class="content_left">
                <!--loginFromStart-->
                <uc3:WhLoginControl ID="WhLoginControl1" runat="server" />
                <!--loginFromEnd-->
                <!--infoListStart-->
                <uc4:WhLineControl ID="WhLineControl1" runat="server" />
                <!--infoListEnd-->
            </div>
            <div class="content_right">
                <!--itemListStart-->
                <div class="moduel_wrap moduel_width1">
                    <h3 class="title">
                        <span class="bg3">用户注册</span></h3>
                    <div class="moduel_wrap1 moduel_wrap1_profile">
                        <table border="0" width="100%">
                            <tbody>
                                <tr>
                                    <td align="right" width="25%">
                                        <font style="color: Red">*</font>省份：
                                    </td>
                                    <td width="75%">
                                        <asp:DropDownList ID="ddlPro" runat="server" valid="required" errmsg="请选择省份!">
                                            <asp:ListItem Value="">-请选择-</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: Red" id="errMsg_<%=ddlPro.ClientID %>" class="errmsg"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <font style="color: Red">*</font> 城市：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCity" runat="server" valid="required" errmsg="请选择城市!">
                                            <asp:ListItem Value="">-请选择-</asp:ListItem>
                                        </asp:DropDownList>
                                        <span style="color: Red" id="errMsg_<%=ddlCity.ClientID %>" class="errmsg"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span style="color: Red">*</span>单位名称：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompanyName" runat="server" valid="required" errmsg="单位名称不为空!"
                                            Width="250px"></asp:TextBox>
                                        <span style="color: Red" id="errMsg_<%=txtCompanyName.ClientID %>" class="errmsg">
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span style="color: Red">*</span>用户名：
                                    </td>
                                    <td>
                                        <div width="100%">
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtUserName" runat="server" valid="required" errmsg="用户名不能为空!" onChange="checkUser()"
                                                    Width="250px"></asp:TextBox>
                                                <span style="color: Red" id="errMsg_<%=txtUserName.ClientID %>" class="errmsg"></span>
                                            </div>
                                            <span class="error_user" style="color: Red; display: none; float: left;">用户名已被使用!</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span style="color: Red">*</span>密码：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUserPwd" runat="server" valid="required" errmsg="密码不能为空!" Width="250px"
                                            TextMode="Password"></asp:TextBox>
                                        <span style="color: Red" id="errMsg_<%=txtUserPwd.ClientID %>" class="errmsg"></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <span style="color: Red">*</span>确认密码：
                                    </td>
                                    <td>
                                        <div width="100%">
                                            <div style="float: left;">
                                                <asp:TextBox ID="txtUserPwdCofirm" runat="server" onchange="checkPassword()" Width="250px"
                                                    TextMode="Password"></asp:TextBox></div>
                                            <span class="error_pwd" style="color: Red; display: none; float: left;">两次密码输入不一致!</span></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        许可证号：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLicense" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        地址：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        邮编：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPostalCode" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        联系人：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContact" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        电话：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTel" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        手机：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhone" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        传真：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFax" runat="server" Width="250px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                        <asp:HiddenField ID="userstate" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lkBtnSave" CssClass="link2btn" runat="server" OnClientClick="return CheckForm()"
                                            OnClick="lkBtnSave_Click">立即注册</asp:LinkButton>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!--itemListEnd-->
            </div>
        </div>
        <!--mainContentEnd-->
        <div class="clearBoth">
        </div>
        <uc2:WhBottomControl ID="WhBottomControl1" runat="server" />
    </div>
    <div id="trMsg" style="color: Red; display: none;">
        <table width="400" border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 10px;">
            <tr>
                <td height="60">
                    <span style="font-size: 14px; font-weight: bold; color: #FF0000; text-indent: 24px;">
                        尊敬的客户： 您已经登录成功 若系统后台没有自动弹出，请点这里进入系统后台！ <a id="linkManage" href="#" style="text-decoration: underline"
                            target="_self">进入系统后台</a></span>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>

    <script type="text/javascript">
        $(function() {
            //验证初始化
            FV_onBlur.initValid($("#<%=lkBtnSave.ClientID %>").closest("form").get(0));
        })



        //省份选择事件
        $("#<%=ddlPro.ClientID %>").change(function() {
            $("#<%=ddlCity.ClientID %>").html("<option value=''>-请选择-</option>");
            //公司ID后台取得
            var id = '<%=companyId %>';
            //参数
            var params = '{proId:"' + $(this).val() + '",companyId:"' + id + '"}';
            $.ajax({
                type: "POST",
                //调用当前页面的BindCity方法
                url: "HotelRouting.aspx/BindCity",
                contentType: "application/json; charset=utf-8",
                //传递的参数
                data: params,
                success: function(msg) {
                    //将返回来的项添加到下拉菜单中 
                    $("#<%=ddlCity.ClientID %>").append(decodeURI(eval('(' + msg + ')').d));
                }
            });
        })

        //验证事件
        function CheckForm() {
            var form = $("#<%=lkBtnSave.ClientID %>").closest("form").get(0);
            //CheckPro();
            //CheckCity();
            var vResult = ValiDatorForm.validator(form, "span");
            if (!vResult || !checkPassword() || checkUser()) {

                return false;
            }
            return true;
        }
        //验证两次密码输入是否相同
        function checkPassword() {
            var pass = $("#<%=txtUserPwd.ClientID %>");
            var passCofirm = $("#<%=txtUserPwdCofirm.ClientID %>");
            if ($.trim(pass.val()) == $.trim(passCofirm.val())) {
                $(".error_pwd").css("display", "none");
                return true;
            } else {
                $(".error_pwd").css("display", "block");
                return false;
            }
        }
        //验证用户名是否重复
        function checkUser() {
            //公司ID后台取得
            var id = '<%=companyId %>';
            //用户名
            var userName = $("#<%=txtUserName.ClientID %>").val();
            //设置参数
            var params = '{userName:"' + userName + '",companyId:"' + id + '"}';
            var res;
            $.ajax({
                type: "POST",
                //调用UsertReg.aspx页面的CheckUserName方法
                url: "UserReg.aspx/CheckUserName",
                contentType: "application/json; charset=utf-8",
                data: params,
                async:false,
                success: function(msg) {

                    res = decodeURI(eval('(' + msg + ')').d);
                    $("#<%=userstate.ClientID %>").val(res);
                    if (res == "false") {
                        $(".error_user").css("display", "none");
                    } else {
                        $(".error_user").css("display", "block");
                    }
                }
            });
            res = $("#<%=userstate.ClientID %>").val();
            if (res == "false") {
                return false;
            } else {
                return true;
            }

        }
    </script>

    </form>
</body>
</html>
