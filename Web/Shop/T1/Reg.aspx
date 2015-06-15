<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reg.aspx.cs" Inherits="Web.Shop.T1.Reg" MasterPageFile="~/Shop/T1/Default.Master" Title="注册" %>
<%--注册--%>
<%@ MasterType VirtualPath="~/Shop/T1/Default.Master" %>

<asp:Content ID="PageHeader" runat="server" ContentPlaceHolderID="PageHead">
<style type="text/css">
    #loginForm input, .content_right input{border: 1px solid #A2C8D2;height: 20px;line-height: 170%;margin-top: 5px;outline: medium none;padding: 0 0 0 2px;}
    body, input, select, button, textarea{font-family: Tahoma,Geneva,sans-serif;font-size: 12px;}
    .moduel_wrap1_profile table{border-collapse: collapse;width: 100%;}
    .moduel_wrap1_profile table, .moduel_wrap1_profile table tr, .moduel_wrap1_profile table tr td{border: 0px solid #ccc;padding: 5px;}
</style>
</asp:content>

<asp:Content ID="PageMain" runat="server" ContentPlaceHolderID="PageMain">
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
                            <asp:DropDownList ID="txtProvince" runat="server">
                                <asp:ListItem Value="">-请选择-</asp:ListItem>
                            </asp:DropDownList>
                            <span style="color: Red; display:none" id="span_<%=txtProvince.ClientID %>">请选择省份</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <font style="color: Red">*</font> 城市：
                        </td>
                        <td>
                            <select id="txtCity" name="txtCity" onchange="selfV()">
                                <option value="">-请选择-</option>
                            </select>
                            <span style="color: Red; display: none" id="span_txtCity">请选择城市</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span style="color: Red">*</span>单位名称：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" runat="server" valid="required" errmsg="单位名称不为空!" Width="250px"></asp:TextBox>
                            <span style="color: Red" id="errMsg_<%=txtCompanyName.ClientID %>" class="errmsg"></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span style="color: Red">*</span>用户名：
                        </td>
                        <td>
                            <div width="100%">
                                <div style="float: left;">
                                    <asp:TextBox ID="txtUserName" runat="server" valid="required" errmsg="用户名不能为空!" onChange="existsUsername()" Width="250px"></asp:TextBox>
                                    <span style="color: Red" id="errMsg_<%=txtUserName.ClientID %>" class="errmsg"></span>
                                </div>
                                <span class="error_user" style="color: Red; display: none; float: left;"> 用户名已被使用!</span></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span style="color: Red">*</span>密码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserPwd" runat="server" valid="required" errmsg="密码不能为空!" Width="250px" TextMode="Password"></asp:TextBox>
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
                                    <asp:TextBox ID="txtUserPwdCofirm" runat="server" onchange="checkPassword()" Width="250px" TextMode="Password"></asp:TextBox></div>
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
                            <asp:LinkButton ID="lkBtnSave" CssClass="link2btn" runat="server" OnClientClick="return WebForm_OnSubmit_Validate()" OnClick="lkBtnSave_Click">立即注册</asp:LinkButton>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <script type="text/javascript" src="/js/ValiDatorForm.js"></script>
    <script type="text/javascript">
        //省份变更
        function changeProvince() {            
            $("#txtCity").html("<option value='0'>-请选择-</option>");
            var params = { provinceid: $("#<%=txtProvince.ClientID %>").val() };
            $.ajax({
                type: "POST",
                url: "default.aspx?doType=getCity",
                data: params,
                success: function(response) {
                    $("#txtCity").append(response);
                }
            });

            if (!selfV()) return;
        }

        //表单验证
        function WebForm_OnSubmit_Validate() {
            
            var form = $("#<%=lkBtnSave.ClientID %>").closest("form").get(0);
            var vResult = ValiDatorForm.validator(form, "span");
            
            if (!selfV() || !vResult || !checkPassword() || existsUsername()) {
                return false;
            }
            
            return true;
        }

        //自定义验证 省份、城市
        function selfV() {
            var selfV = true;

            if ($("#<%=txtProvince.ClientID %>").val().length < 1 || $("#<%=txtProvince.ClientID %>").val() == "0") {
                $("#span_<%=txtProvince.ClientID %>").show();
                selfV = false;
            } else {
                $("#span_<%=txtProvince.ClientID %>").hide();
            }

            if ($("#txtCity").val().length < 1 || $("#txtCity").val() == "0") {
                $("#span_txtCity").show();
                selfV = false;
            } else {
                $("#span_txtCity").hide();
            }

            return selfV;
        }
        
        //验证两次密码输入是否相同 返回true相同 
        function checkPassword() {
            var pwd = $.trim($("#<%=txtUserPwd.ClientID %>").val());
            var confirm = $.trim($("#<%=txtUserPwdCofirm.ClientID %>").val());
            if(pwd.length<1) return false;
            if(pwd==confirm) {         
                $(".error_pwd").css("display", "none");
                return true;
            } else {
                $(".error_pwd").css("display", "block");
                return false;
            }
        }
        //验证用户名是否重复 返回true存在相同用户名 
        function existsUsername() {
            var params = { username: $("#<%=txtUserName.ClientID %>").val() };
            var retCode = false;
            $.ajax({
                type: "POST",
                url: "default.aspx?doType=existsUsername",
                data: params,
                async: false,
                success: function(response) {
                    if (response == "0") {
                        $(".error_user").css("display", "none");
                        retCode= false;
                    } else {
                        $(".error_user").css("display", "block");
                        retCode= true;
                    }
                }
            });
            
            return retCode;
        }

        $(document).ready(function() {            
            $("#<%=txtProvince.ClientID %>").bind("change", function() { changeProvince(); });
            FV_onBlur.initValid($("#<%=lkBtnSave.ClientID %>").closest("form").get(0));
        });
    </script>
    
</asp:Content>
